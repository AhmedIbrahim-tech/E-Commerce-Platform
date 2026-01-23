using Domain.Common.Base;
using Domain.Common.Interfaces;
using Domain.Entities.Shipping;

namespace Domain.Entities.Orders;

public class Order : BaseEntity, IAuditable
{
    public DateTimeOffset OrderDate { get; set; }
    public Status Status { get; set; }
    public decimal TotalAmount { get; set; }
    public string? PaymentToken { get; set; }

    public DateTimeOffset CreatedTime { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTimeOffset? ModifiedTime { get; set; }
    public Guid? ModifiedBy { get; set; }

    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public Guid? ShippingAddressId { get; set; }
    public ShippingAddress? ShippingAddress { get; set; }
    public Guid? PaymentId { get; set; }
    public Payment? Payment { get; set; }
    public Guid? DeliveryId { get; set; }
    public Delivery? Delivery { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; }

    public Order()
    {
        OrderItems = new HashSet<OrderItem>();
        OrderDate = DateTimeOffset.UtcNow;
        CreatedTime = DateTimeOffset.UtcNow;
    }
}
