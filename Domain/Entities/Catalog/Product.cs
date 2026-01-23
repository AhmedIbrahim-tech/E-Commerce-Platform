using Domain.Common.Base;
using Domain.Common.Interfaces;
using Domain.Enums;

namespace Domain.Entities.Catalog;

public class Product : BaseEntity, IAuditable, ISoftDelete
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ShortDescription { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int QuantityAlert { get; set; }
    public string? Barcode { get; set; }
    public string? BarcodeSymbology { get; set; }
    public DateTimeOffset? ManufacturedDate { get; set; }
    public DateTimeOffset? ExpiryDate { get; set; }
    public string? Manufacturer { get; set; }
    public bool IsActive { get; set; } = true;
    public ProductPublishStatus PublishStatus { get; set; } = ProductPublishStatus.Published;
    public ProductVisibility Visibility { get; set; } = ProductVisibility.Public;
    public DateTimeOffset? PublishDate { get; set; }

    public DateTimeOffset CreatedTime { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTimeOffset? ModifiedTime { get; set; }
    public Guid? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedTime { get; set; }
    public Guid? DeletedBy { get; set; }

    public ProductType ProductType { get; set; } = ProductType.Single;
    public SellingType SellingType { get; set; } = SellingType.Both;
    public TaxType? TaxType { get; set; }
    public decimal? TaxRate { get; set; }
    public DiscountType? DiscountType { get; set; }
    public decimal? DiscountValue { get; set; }

    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }

    public Guid? SubCategoryId { get; set; }
    public SubCategory? SubCategory { get; set; }

    public Guid? BrandId { get; set; }
    public Brand? Brand { get; set; }

    public Guid? UnitOfMeasureId { get; set; }
    public UnitOfMeasure? UnitOfMeasure { get; set; }

    public Guid? WarrantyId { get; set; }
    public Warranty? Warranty { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; }
    public ICollection<Review> Reviews { get; set; }
    public ICollection<ProductImage> ProductImages { get; set; }
    public ICollection<ProductVariant> ProductVariants { get; set; }
    public ICollection<ProductTag> ProductTags { get; set; }

    public Product()
    {
        OrderItems = new HashSet<OrderItem>();
        Reviews = new HashSet<Review>();
        ProductImages = new HashSet<ProductImage>();
        ProductVariants = new HashSet<ProductVariant>();
        ProductTags = new HashSet<ProductTag>();
        CreatedTime = DateTimeOffset.UtcNow;
    }

    public void MarkDeleted(Guid userId)
    {
        IsDeleted = true;
        DeletedTime = DateTimeOffset.UtcNow;
        DeletedBy = userId;
    }
}
