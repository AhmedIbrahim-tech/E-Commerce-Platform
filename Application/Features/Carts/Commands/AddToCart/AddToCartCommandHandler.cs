using Application.Common.Bases;
using Application.Common.Errors;
using Domain.Entities;
using Infrastructure.RepositoriesHandlers.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace Application.Features.Carts.Commands.AddToCart;

public class AddToCartCommandHandler(
    IMemoryCache memoryCache,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService) : ApiResponseHandler(),
    IRequestHandler<AddToCartCommand, ApiResponse<string>>
{

    public async Task<ApiResponse<string>> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var ownerId = currentUserService.GetCartOwnerId();
            var cartKey = $"cart:{ownerId}";
            var existingCart = GetCartByKey(cartKey) ?? new Cart
            {
                CustomerId = ownerId,
                CreatedAt = DateTimeOffset.UtcNow.ToLocalTime(),
                CartItems = new List<CartItem>(),
                TotalAmount = 0,
            };

            var existingProduct = await unitOfWork.Products.GetTableNoTracking()
                .Where(c => c.Id.Equals(request.ProductId))
                .Include(c => c.Category)
                .FirstOrDefaultAsync(cancellationToken);
            if (existingProduct is null) return new ApiResponse<string>(ProductErrors.ProductNotFound());

            var existingItem = existingCart.CartItems?.FirstOrDefault(x => x.ProductId == request.ProductId);
            if (existingItem != null)
                return new ApiResponse<string>(CartErrors.ProductAlreadyInCart());

            existingCart.CartItems!.Add(new CartItem
            {
                CartId = existingCart.CustomerId,
                ProductId = request.ProductId,
                Price = existingProduct.Price,
                Quantity = request.Quantity,
                CreatedAt = DateTimeOffset.UtcNow.ToLocalTime(),
                SubAmount = existingProduct.Price * request.Quantity
            });
            existingCart.TotalAmount = (existingCart.TotalAmount ?? 0) + (existingProduct.Price ?? 0) * request.Quantity;

            var result = AddOrEditCart(existingCart);
            if (result is null) return new ApiResponse<string>(CartErrors.CannotModifyCart());

            return Success<string>("Added to cart");
        }
        catch (Exception)
        {
            return new ApiResponse<string>(CartErrors.InvalidCartOperation());
        }
    }

    public Cart? AddOrEditCart(Cart cart)
    {
        var cartKey = $"cart:{cart.CustomerId}";
        var existingCart = GetCartByKey(cartKey);

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
        memoryCache.Set(cartKey, serialized, options);
        return GetCartByKey(cartKey);
    }

    public Cart? GetCartByKey(string cartKey)
    {
        if (!memoryCache.TryGetValue(cartKey, out string? cached))
            return null;
        return JsonSerializer.Deserialize<Cart>(cached ?? "");
    }

}

