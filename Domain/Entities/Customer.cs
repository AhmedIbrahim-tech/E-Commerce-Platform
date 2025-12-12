namespace Domain.Entities;

public class Customer
{
    public Guid Id { get; set; }
    public string? FullName { get; set; }
    public Gender? Gender { get; set; }
    public Guid AppUserId { get; set; }
    public ICollection<Order> Orders { get; set; }
    public ICollection<ShippingAddress> ShippingAddresses { get; set; }
    public ICollection<Review> Reviews { get; set; }

    public Customer()
    {
        Orders = new HashSet<Order>();
        ShippingAddresses = new HashSet<ShippingAddress>();
        Reviews = new HashSet<Review>();
    }
}
