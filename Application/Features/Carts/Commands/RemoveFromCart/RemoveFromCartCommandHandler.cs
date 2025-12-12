using Application.Common.Bases;
using Application.Common.Errors;
using Domain.Entities;
using Infrastructure.RepositoriesHandlers.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace Application.Features.Carts.Commands.RemoveFromCart;

public class RemoveFromCartCommandHandler(
    IMemoryCache memoryCache,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService) : ApiResponseHandler(),
    IRequestHandler<RemoveFromCartCommand, ApiResponse<string>>
{
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

    public async Task<ApiResponse<string>> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var cartKey = $"cart:{currentUserService.GetCartOwnerId()}";
            var existingCart = GetCartByKey(cartKey);
            if (existingCart is null) return new ApiResponse<string>(CartErrors.CartNotFound());

            var existingProduct = await unitOfWork.Products.GetTableNoTracking()
                .Where(c => c.Id.Equals(request.ProductId))
                .Include(c => c.Category)
                .FirstOrDefaultAsync(cancellationToken);
            if (existingProduct is null) return new ApiResponse<string>(ProductErrors.ProductNotFound());

            var itemToRemove = existingCart.CartItems?.FirstOrDefault(x => x.ProductId == request.ProductId);
            if (itemToRemove is null) return new ApiResponse<string>(CartErrors.CartItemNotFound());

            existingCart.CartItems!.Remove(itemToRemove);
            existingCart.TotalAmount = (existingCart.TotalAmount ?? 0) - (itemToRemove.SubAmount ?? 0);

            var result = AddOrEditCart(existingCart);
            if (result is null) return new ApiResponse<string>(CartErrors.CannotModifyCart());

            return Success<string>("Item removed from cart");
        }
        catch (Exception)
        {
            return new ApiResponse<string>(CartErrors.InvalidCartOperation());
        }
    }
}

