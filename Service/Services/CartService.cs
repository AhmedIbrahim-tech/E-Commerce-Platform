
using Microsoft.Extensions.Caching.Memory;

namespace Service.Services
{
    public class CartService : ICartService
    {
        #region Fields
        private readonly IMemoryCache _memoryCache;
        private readonly ICurrentUserService _currentUserService;
        private readonly IProductService _productService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Constructors
        public CartService(IMemoryCache memoryCache,
            ICurrentUserService currentUserService,
            IProductService productService,
            IHttpContextAccessor httpContextAccessor)
        {
            _memoryCache = memoryCache;
            _currentUserService = currentUserService;
            _productService = productService;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Private Helpers
        private string GetCartKey() => $"cart:{_currentUserService.GetCartOwnerId()}";
        #endregion

        #region Handle Functions
        public async Task<Cart?> GetCartByKeyAsync(string cartKey)
        {
            if (!_memoryCache.TryGetValue(cartKey, out string? cached))
            {
                return null;
            }

            return JsonSerializer.Deserialize<Cart>(cached ?? "");
        }

        // Add or Update Cart
        public async Task<Cart?> AddOrEditCartAsync(Cart cart)
        {
            var cartKey = $"cart:{cart.CustomerId}";
            var existingCart = await GetCartByKeyAsync(cartKey);

            if (existingCart is not null)
            {
                cart.CreatedAt = cart.CreatedAt == default ? existingCart.CreatedAt : DateTimeOffset.UtcNow.ToLocalTime();
                cart.CustomerId = cart.CustomerId == Guid.Empty ? existingCart.CustomerId : cart.CustomerId;
                cart.CartItems = cart.CartItems ?? existingCart.CartItems;
                cart.TotalAmount = cart.TotalAmount ?? existingCart.TotalAmount;
                cart.PaymentToken = string.IsNullOrEmpty(cart.PaymentToken) ? existingCart.PaymentToken : cart.PaymentToken;
                cart.PaymentIntentId = string.IsNullOrEmpty(cart.PaymentIntentId) ? existingCart.PaymentIntentId : cart.PaymentIntentId;
            }

            var serialized = JsonSerializer.Serialize(cart);

            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(3)
            };

            _memoryCache.Set(cartKey, serialized, options);
            return await GetCartByKeyAsync(cartKey);
        }

        public async Task<Cart?> GetMyCartAsync()
        {
            var cartKey = GetCartKey();
            return await GetCartByKeyAsync(cartKey);
        }

        public async Task<string> AddToCartAsync(Guid productId, int quantity)
        {
            try
            {
                var ownerId = _currentUserService.GetCartOwnerId();
                var cartKey = $"cart:{ownerId}";
                var existingCart = await GetCartByKeyAsync(cartKey) ?? new Cart
                {
                    CustomerId = ownerId,
                    CreatedAt = DateTimeOffset.UtcNow.ToLocalTime(),
                    CartItems = new List<CartItem>(),
                    TotalAmount = 0,
                };
                // Check if the product exists
                var existingProduct = await _productService.GetProductByIdAsync(productId);
                if (existingProduct is null) return "ProductNotFound";
                // Check if the item already exists in the cart
                var existingItem = existingCart.CartItems?.FirstOrDefault(x => x.ProductId == productId);
                if (existingItem != null)
                    return "ItemAlreadyExistsInCart";
                else
                {
                    // Add new item to the cart
                    existingCart.CartItems!.Add(new CartItem
                    {
                        CartId = existingCart.CustomerId,
                        ProductId = productId,
                        Price = existingProduct.Price,
                        Quantity = quantity,
                        CreatedAt = DateTimeOffset.UtcNow.ToLocalTime(),
                        SubAmount = existingProduct.Price * quantity
                    });
                    existingCart.TotalAmount += existingProduct.Price * quantity;
                }
                // Save the updated cart
                var result = await AddOrEditCartAsync(existingCart);
                if (result is null) return "FailedInAddItemToCart";
                return "Success";
            }
            catch (Exception)
            {
                return "AnErrorOccurredWhileAddingToTheCart";
            }
        }

        public async Task<string> RemoveItemFromCartAsync(Guid productId)
        {
            try
            {
                var existingCart = await GetMyCartAsync();
                if (existingCart is null) return "CartNotFound";
                // Check if the product exists
                var existingProduct = await _productService.GetProductByIdAsync(productId);
                if (existingProduct is null) return "ProductNotFound";
                // Find the item to remove
                var itemToRemove = existingCart.CartItems?.FirstOrDefault(x => x.ProductId == productId);
                if (itemToRemove is null) return "ItemNotFoundInCart";
                // Remove the item
                existingCart.CartItems!.Remove(itemToRemove);
                // Save the updated cart
                var result = await AddOrEditCartAsync(existingCart);
                if (result is null) return "FailedInRemoveItemFromCart";
                return "Success";
            }
            catch (Exception)
            {
                return "AnErrorOccurredWhileRemovingItemFromTheCart";
            }
        }

        public async Task<string> UpdateItemQuantityAsync(Guid productId, int Quantity)
        {
            try
            {
                var existingCart = await GetMyCartAsync();
                if (existingCart is null) return "CartNotFound";
                // Check if the product exists
                var existingProduct = await _productService.GetProductByIdAsync(productId);
                if (existingProduct is null) return "ProductNotFound";
                // Find the item to update
                var itemToUpdate = existingCart.CartItems?.FirstOrDefault(x => x.ProductId == productId);
                if (itemToUpdate is null) return "ItemNotFoundInCart";
                // Update the quantity
                itemToUpdate.Quantity = Quantity;
                // Save the updated cart
                var result = await AddOrEditCartAsync(existingCart);
                if (result is null) return "FailedInUpdateItemQuantity";
                return "Success";
            }
            catch (Exception)
            {
                return "AnErrorOccurredWhileUpdatingItemQuantityInTheCart";
            }
        }

        public async Task<bool> DeleteMyCartAsync()
        {
            try
            {
                var cartKey = GetCartKey();
                _memoryCache.Remove(cartKey);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteCartAsync(Guid customerId)
        {
            try
            {
                var cartKey = $"cart:{customerId}";
                _memoryCache.Remove(cartKey);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<string> MigrateGuestCartToCustomerAsync(Guid customerId)
        {
            try
            {
                var guestId = _httpContextAccessor.HttpContext?.Request.Cookies["GuestId"];

                var guestCartKey = $"cart:{guestId}";
                var userCartKey = $"cart:{customerId}";

                var guestCart = await GetCartByKeyAsync(guestCartKey);
                if (guestCart != null)
                {
                    guestCart.CustomerId = customerId;
                    var result1 = await AddOrEditCartAsync(guestCart);
                    if (result1 is null) return "FailedInEditCart";
                    await DeleteCartAsync(Guid.Parse(guestId!));
                }

                // Delete the guest id from Cookies
                var result2 = _currentUserService.DeleteGuestIdCookie();
                if (!result2)
                {
                    return "FailedToDeleteGuestIdCookie";
                }

                return "Success";
            }
            catch (Exception)
            {
                return "FailedInMigrateGuestCartToCustomer";
            }
        }
        #endregion
    }
}
