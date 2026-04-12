using Domain.Common.Base;

namespace Domain.Entities.Shipping;

public class ShippingAddress : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;

    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }
}
