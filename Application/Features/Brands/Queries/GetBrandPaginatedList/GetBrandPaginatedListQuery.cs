namespace Application.Features.Brands.Queries.GetBrandPaginatedList;

public record GetBrandPaginatedListQuery(int PageNumber, int PageSize, string? Search,
    BrandSortingEnum SortBy) : IRequest<PaginatedResult<GetBrandPaginatedListResponse>>;
