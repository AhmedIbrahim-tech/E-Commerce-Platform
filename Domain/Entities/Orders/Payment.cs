using Domain.Common.Base;

namespace Domain.Entities.Orders;

public class Payment : BaseEntity
{
    public Guid OrderId { get; set; }
    public string? TransactionId { get; set; }
    public DateTimeOffset? PaymentDate { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public decimal? TotalAmount { get; set; }
    public Status Status { get; set; }
}
