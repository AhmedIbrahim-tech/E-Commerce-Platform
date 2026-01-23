namespace Application.Features.Products.Queries.GetProductPaginatedList;

public record GetProductPaginatedListQuery(
    int PageNumber,
    int PageSize,
    string? Search,
    ProductSortingEnum SortBy,
    Guid? CategoryId = null,
    List<Guid>? BrandIds = null,
    bool? IsActive = null,
    decimal? MinPrice = null,
    decimal? MaxPrice = null,
    decimal? MinDiscountPercentage = null,
    int? MinRating = null) : IRequest<PaginatedResult<GetProductPaginatedListResponse>>;

