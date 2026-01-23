using Application.Common.Bases;
using Application.Features.Products.Commands.AddProduct;
using Domain.Enums;

namespace Application.Features.Products.Commands.EditProduct;

public record EditProductCommand
(
    Guid Id,
    string Name,
    string Slug,
    string SKU,
    string? Description,
    decimal Price,
    int StockQuantity,
    int QuantityAlert,
    string? Barcode,
    string? BarcodeSymbology,
    ProductType ProductType,
    SellingType SellingType,
    TaxType? TaxType,
    decimal? TaxRate,
    DiscountType? DiscountType,
    decimal? DiscountValue,
    Guid CategoryId,
    Guid? SubCategoryId,
    Guid? BrandId,
    Guid? UnitOfMeasureId,
    Guid? WarrantyId,
    DateTimeOffset? ManufacturedDate,
    DateTimeOffset? ExpiryDate,
    string? Manufacturer,
    bool IsActive,
    List<ProductImageDto>? ProductImages,
    List<ProductVariantDto>? ProductVariants,
    string? ShortDescription = null,
    ProductPublishStatus PublishStatus = ProductPublishStatus.Published,
    ProductVisibility Visibility = ProductVisibility.Public,
    DateTimeOffset? PublishDate = null,
    List<string>? Tags = null,
    bool ReplaceTags = true
) : IRequest<ApiResponse<string>>;

