namespace Application.Features.Categories.Queries.GetCategoryPaginatedList;

public record GetCategoryPaginatedListQuery(int PageNumber, int PageSize, string? Search,
    CategorySortingEnum SortBy) : IRequest<PaginatedResult<GetCategoryPaginatedListResponse>>;

