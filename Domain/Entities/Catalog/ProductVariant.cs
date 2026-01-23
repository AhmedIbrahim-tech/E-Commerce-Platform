using Domain.Common.Base;
using Domain.Common.Interfaces;

namespace Domain.Entities.Catalog;

public class ProductVariant : BaseEntity, IAuditable, ISoftDelete
{
    public string VariantAttribute { get; set; } = string.Empty;
    public string VariantValue { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string? ImageURL { get; set; }
    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedTime { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTimeOffset? ModifiedTime { get; set; }
    public Guid? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedTime { get; set; }
    public Guid? DeletedBy { get; set; }

    public Guid ProductId { get; set; }
    public Product? Product { get; set; }

    public ProductVariant()
    {
        CreatedTime = DateTimeOffset.UtcNow;
    }

    public void MarkDeleted(Guid userId)
    {
        IsDeleted = true;
        DeletedTime = DateTimeOffset.UtcNow;
        DeletedBy = userId;
    }
}
