using System.Text.Json;

namespace Application.Features.Carts.Commands.UpdateItemQuantity;

public class UpdateItemQuantityCommandHandler(
    IMemoryCache memoryCache,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService) : ApiResponseHandler(),
    IRequestHandler<UpdateItemQuantityCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(UpdateItemQuantityCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Quantity <= 0)
                return new ApiResponse<string>(CartErrors.InvalidQuantity());

            var cartKey = $"cart:{currentUserService.GetCartOwnerId()}";
            var existingCart = GetCartByKey(cartKey);
            if (existingCart is null) return new ApiResponse<string>(CartErrors.CartNotFound());

            var existingProduct = await unitOfWork.Products.GetTableNoTracking()
                .Where(c => c.Id.Equals(request.ProductId))
                .Include(c => c.Category)
                .FirstOrDefaultAsync(cancellationToken);
            if (existingProduct is null) return new ApiResponse<string>(ProductErrors.ProductNotFound());

            var itemToUpdate = existingCart.CartItems?.FirstOrDefault(x => x.ProductId == request.ProductId);
            if (itemToUpdate is null) return new ApiResponse<string>(CartErrors.CartItemNotFound());

            var oldSubAmount = itemToUpdate.SubAmount ?? 0;
            itemToUpdate.Quantity = request.Quantity;
            itemToUpdate.SubAmount = (existingProduct.Price ?? 0) * request.Quantity;
            existingCart.TotalAmount = (existingCart.TotalAmount ?? 0) - oldSubAmount + (itemToUpdate.SubAmount ?? 0);

            var result = AddOrEditCart(existingCart);
            if (result is null) return new ApiResponse<string>(CartErrors.CannotModifyCart());

            return Success<string>("Item quantity updated");
        }
        catch (Exception)
        {
            return new ApiResponse<string>(CartErrors.InvalidQuantity());
        }
    }


    private Cart? GetCartByKey(string cartKey)
    {
        if (!memoryCache.TryGetValue(cartKey, out string? cached))
            return null;
        return JsonSerializer.Deserialize<Cart>(cached ?? "");
    }

    private Cart? AddOrEditCart(Cart cart)
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

}

