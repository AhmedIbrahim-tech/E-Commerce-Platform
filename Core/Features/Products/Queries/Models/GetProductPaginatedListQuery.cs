using Core.Features.Products.Queries.Responses;

namespace Core.Features.Products.Queries.Models
{
    public record GetProductPaginatedListQuery(int PageNumber, int PageSize, string? Search,
        ProductSortingEnum SortBy) : IRequest<PaginatedResult<GetProductPaginatedListResponse>>;
}
