using Domain.Enums;

namespace Application.Features.Products.Queries.GetProductById;

public record GetProductByIdResponse(
    Guid Id,
    string Name,
    string Slug,
    string SKU,
    string? Description,
    string? ShortDescription,
    decimal Price,
    int StockQuantity,
    int QuantityAlert,
    string? Barcode,
    string? BarcodeSymbology,
    ProductPublishStatus PublishStatus,
    ProductVisibility Visibility,
    DateTimeOffset? PublishDate,
    ProductType ProductType,
    SellingType SellingType,
    TaxType? TaxType,
    decimal? TaxRate,
    DiscountType? DiscountType,
    decimal? DiscountValue,
    Guid CategoryId,
    string? CategoryName,
    Guid? SubCategoryId,
    string? SubCategoryName,
    Guid? BrandId,
    string? BrandName,
    Guid? UnitOfMeasureId,
    string? UnitOfMeasureName,
    Guid? WarrantyId,
    string? WarrantyName,
    DateTimeOffset? ManufacturedDate,
    DateTimeOffset? ExpiryDate,
    string? Manufacturer,
    bool IsActive,
    DateTimeOffset CreatedTime,
    DateTimeOffset? ModifiedTime,
    List<ProductImageResponse>? ProductImages,
    List<ProductVariantResponse>? ProductVariants,
    List<string>? Tags)
{
    public PaginatedResult<ReviewResponse>? Reviews { get; set; }
}

public record ProductImageResponse(
    Guid Id,
    string ImageURL,
    bool IsPrimary,
    int DisplayOrder);

public record ProductVariantResponse(
    Guid Id,
    string VariantAttribute,
    string VariantValue,
    string SKU,
    int Quantity,
    decimal Price,
    string? ImageURL,
    bool IsActive);

public record ReviewResponse(
    Guid CustomerId,
    string? FullName,
    Rating Rating,
    string? Comment);

