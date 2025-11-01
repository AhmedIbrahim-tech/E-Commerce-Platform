using Core.Features.Categories.Queries.Response;

namespace Core.Features.Categories.Queries.Models
{
    public record GetCategoryPaginatedListQuery(int PageNumber, int PageSize, string? Search,
    CategorySortingEnum SortBy) : IRequest<PaginatedResult<GetCategoryPaginatedListResponse>>;
}
