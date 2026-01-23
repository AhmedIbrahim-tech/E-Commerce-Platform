using Domain.Common.Base;

namespace Domain.Entities.Orders;

public class Delivery : BaseEntity
{
    public DeliveryMethod DeliveryMethod { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset? DeliveryTime { get; set; }
    public decimal? Cost { get; set; }
    public Status? Status { get; set; }
}
