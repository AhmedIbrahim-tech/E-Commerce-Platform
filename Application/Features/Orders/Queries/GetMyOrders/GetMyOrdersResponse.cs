namespace Application.Features.Orders.Queries.GetMyOrders;

public record GetMyOrdersResponse(
    Guid Id,
    DateTimeOffset? OrderDate,
    Status? OrderStatus,
    decimal? TotalAmount,
    string? CustomerName,
    string? ShippingAddress,
    PaymentMethod? PaymentMethod,
    DateTimeOffset? PaymentDate,
    Status? PaymentStatus,
    DeliveryMethod? DeliveryMethod,
    DateTimeOffset? DeliveryTime,
    decimal? DeliveryCost,
    Status? DeliveryStatus);

