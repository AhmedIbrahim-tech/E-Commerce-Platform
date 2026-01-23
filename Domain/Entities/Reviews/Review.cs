using Domain.Common.Base;

namespace Domain.Entities.Reviews;

public class Review
{
    /// Composite Primary Key <CustomerId, ProductId>
    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }

    public Rating Rating { get; set; }
    public string? Comment { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
}
