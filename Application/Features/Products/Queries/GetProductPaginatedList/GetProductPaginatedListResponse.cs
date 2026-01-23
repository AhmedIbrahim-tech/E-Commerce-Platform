using Domain.Enums;

namespace Application.Features.Products.Queries.GetProductPaginatedList;

public record GetProductPaginatedListResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Slug { get; init; }
    public string SKU { get; init; }
    public string? Description { get; init; }
    public string? ShortDescription { get; init; }
    public decimal Price { get; init; }
    public int StockQuantity { get; init; }
    public int QuantityAlert { get; init; }
    public string? Barcode { get; init; }
    public ProductType ProductType { get; init; }
    public SellingType SellingType { get; init; }
    public ProductPublishStatus PublishStatus { get; init; }
    public ProductVisibility Visibility { get; init; }
    public DateTimeOffset? PublishDate { get; init; }
    public bool IsActive { get; init; }
    public DateTimeOffset CreatedTime { get; init; }
    public string? CategoryName { get; init; }
    public string? SubCategoryName { get; init; }
    public string? BrandName { get; init; }
    public string? WarrantyName { get; init; }
    public List<ProductImageResponse>? ProductImages { get; init; }
}

public record ProductImageResponse(
    Guid Id,
    string ImageURL,
    bool IsPrimary,
    int DisplayOrder);
