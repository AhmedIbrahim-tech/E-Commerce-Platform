namespace Application.Features.Orders.Queries.GetMyOrders;

public record GetMyOrdersQuery(int PageNumber, int PageSize, string? Search,
    OrderSortingEnum SortBy) : IRequest<PaginatedResult<GetMyOrdersResponse>>;

