namespace Application.Features.Products.Queries.GetProductPaginatedList;

public record GetProductPaginatedListQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? Search = null,
    ProductSortingEnum SortBy = ProductSortingEnum.NameAsc,
    Guid? CategoryId = null,
    List<Guid>? BrandIds = null,
    bool? IsActive = null,
    decimal? MinPrice = null,
    decimal? MaxPrice = null,
    decimal? MinDiscountPercentage = null,
    int? MinRating = null) : IRequest<PaginatedResult<GetProductPaginatedListResponse>>;

