namespace Application.Features.Orders.Queries.GetOrderPaginatedList;

public record GetOrderPaginatedListQuery(int PageNumber, int PageSize, string? Search,
    OrderSortingEnum SortBy) : IRequest<PaginatedResult<GetOrderPaginatedListResponse>>;

