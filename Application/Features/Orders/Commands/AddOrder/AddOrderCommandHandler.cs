using Application.Common.Bases;
using Domain.Entities;
using Infrastructure.RepositoriesHandlers.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace Application.Features.Orders.Commands.AddOrder;

public class AddOrderCommandHandler(
    IUnitOfWork unitOfWork,
    IMemoryCache memoryCache,
    ICurrentUserService currentUserService,
    IHttpContextAccessor httpContextAccessor) : ApiResponseHandler(),
    IRequestHandler<AddOrderCommand, Guid>
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

    private string MigrateGuestCartToCustomer(Guid customerId)
    {
        try
        {
            var guestId = httpContextAccessor.HttpContext?.Request.Cookies["GuestId"];
            var guestCartKey = $"cart:{guestId}";
            var guestCart = GetCartByKey(guestCartKey);
            
            if (guestCart != null)
            {
                guestCart.CustomerId = customerId;
                var result1 = AddOrEditCart(guestCart);
                if (result1 is null) return "FailedInEditCart";
                memoryCache.Remove(guestCartKey);
            }

            var result2 = currentUserService.DeleteGuestIdCookie();
            if (!result2)
                return "FailedToDeleteGuestIdCookie";

            return "Success";
        }
        catch (Exception)
        {
            return "FailedInMigrateGuestCartToCustomer";
        }
    }

    public async Task<Guid> Handle(AddOrderCommand request, CancellationToken cancellationToken)
    {
        // Check if the user is authenticated
        if (!currentUserService.IsAuthenticated)
            throw new InvalidOperationException("Please login first");

        // Retrieve the cart
        var userId = currentUserService.GetCartOwnerId();
        var cartKey = $"cart:{userId}";

        var result1 = MigrateGuestCartToCustomer(userId);
        var badRequestMessage = result1 switch
        {
            "FailedInEditCart" => "Failed to modify this cart",
            "TransactionFailedToCommit" => "Transaction failed to commit",
            "FailedInMigrateGuestCartToCustomer" => "Failed to migrate guest cart to customer",
            "FailedToDeleteGuestIdCookie" => "Failed to delete guest ID cookie",
            _ => null
        };

        if (badRequestMessage != null)
            BadRequest<string>(badRequestMessage);

        var cart = GetCartByKey(cartKey);
        if (cart == null || cart.CartItems?.Count == 0)
            throw new InvalidOperationException("Cart not found or empty");

        var order = new Order();

        // Validate and process each cart item
        foreach (var item in cart.CartItems!)
        {
            var product = await unitOfWork.Products.GetTableNoTracking()
                .Where(c => c.Id.Equals(item.ProductId))
                .Include(c => c.Category)
                .FirstOrDefaultAsync(cancellationToken);

            if (product == null)
                throw new InvalidOperationException($"Product with ID {item.ProductId} does not exist.");

            if (product.Price == null || product.StockQuantity < item.Quantity)
                throw new InvalidOperationException($"Product {product.Name} is not available or stock is insufficient.");

            order.OrderItems.Add(new OrderItem
            {
                ProductId = item.ProductId,
                OrderId = order.Id,
                Quantity = item.Quantity,
                UnitPrice = product.Price,
                SubAmount = item.Quantity * (product.Price ?? 0)
            });
        }

        order.CustomerId = cart.CustomerId;
        order.OrderDate = DateTimeOffset.UtcNow.ToLocalTime();
        order.TotalAmount = order.OrderItems.Sum(i => i.SubAmount);
        order.Status = Status.Draft;

        // Add Order and return result
        try
        {
            await unitOfWork.Orders.AddAsync(order);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return order.Id;
        }
        catch (Exception)
        {
            throw new InvalidOperationException("Creation failed");
        }
    }
}

