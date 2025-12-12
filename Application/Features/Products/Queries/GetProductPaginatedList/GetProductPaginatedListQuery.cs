namespace Application.Features.Products.Queries.GetProductPaginatedList;

public record GetProductPaginatedListQuery(int PageNumber, int PageSize, string? Search,
    ProductSortingEnum SortBy) : IRequest<PaginatedResult<GetProductPaginatedListResponse>>;

