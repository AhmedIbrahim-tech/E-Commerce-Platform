using Application.Common.Bases;
using Domain.Entities.Cart;
using Infrastructure.RepositoriesHandlers.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace Application.Features.Orders.Commands.AddOrder;

public class AddOrderCommandHandler(
    IUnitOfWork unitOfWork,
    IMemoryCache memoryCache,
    ICurrentUserService currentUserService,
    INotificationService notificationService,
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
            cart.CreatedTime = cart.CreatedTime == default ? existingCart.CreatedTime : DateTimeOffset.UtcNow.ToLocalTime();
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
        var possibleMerchantAppUserIds = new HashSet<Guid>();

        // Validate and process each cart item
        foreach (var item in cart.CartItems!)
        {
            var product = await unitOfWork.Products.GetTableNoTracking()
                .Where(c => c.Id.Equals(item.ProductId))
                .Include(c => c.Category)
                .FirstOrDefaultAsync(cancellationToken);

            if (product == null)
                throw new InvalidOperationException($"Product with ID {item.ProductId} does not exist.");

            if (product.CreatedBy != Guid.Empty)
                possibleMerchantAppUserIds.Add(product.CreatedBy);

            var quantity = item.Quantity ?? 0;
            if (quantity <= 0 || product.StockQuantity < quantity)
                throw new InvalidOperationException($"Product {product.Name} is not available or stock is insufficient.");

            order.OrderItems.Add(new OrderItem
            {
                ProductId = item.ProductId,
                OrderId = order.Id,
                Quantity = quantity,
                UnitPrice = product.Price,
                SubAmount = quantity * product.Price
            });
        }

        order.CustomerId = cart.CustomerId;
        order.OrderDate = DateTimeOffset.UtcNow.ToLocalTime();
        order.TotalAmount = order.OrderItems.Sum(i => i.SubAmount ?? 0);
        order.Status = Status.Draft;

        // Add Order and return result
        try
        {
            await unitOfWork.Orders.AddAsync(order, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var merchantIds = await unitOfWork.Context.Vendors.AsNoTracking()
                .Where(v => possibleMerchantAppUserIds.Contains(v.AppUserId))
                .Select(v => v.AppUserId)
                .Distinct()
                .ToListAsync(cancellationToken);

            var adminIds = await unitOfWork.Context.Vendors.AsNoTracking()
                .Where(v => merchantIds.Contains(v.AppUserId))
                .Select(v => v.CreatedBy)
                .Distinct()
                .Join(
                    unitOfWork.Context.Admins.AsNoTracking(),
                    createdBy => createdBy,
                    admin => admin.AppUserId,
                    (createdBy, admin) => admin.AppUserId)
                .Distinct()
                .ToListAsync(cancellationToken);

            await notificationService.CreateAsync(
                "new_order",
                new { orderId = order.Id, totalAmount = order.TotalAmount, status = order.Status.ToString() },
                new NotificationRecipients(
                    AdminIds: adminIds,
                    MerchantIds: merchantIds),
                cancellationToken);

            return order.Id;
        }
        catch (Exception)
        {
            throw new InvalidOperationException("Creation failed");
        }
    }
}

