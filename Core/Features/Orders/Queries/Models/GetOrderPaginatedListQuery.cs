using Core.Features.Orders.Queries.Responses;

namespace Core.Features.Orders.Queries.Models
{
    public record GetOrderPaginatedListQuery(int PageNumber, int PageSize, string? Search,
        OrderSortingEnum SortBy) : IRequest<PaginatedResult<GetOrderPaginatedListResponse>>;
}
