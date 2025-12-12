namespace Application.Features.Customers.Queries.GetCustomerPaginatedList;

public record GetCustomerPaginatedListQuery(int PageNumber, int PageSize, string? Search,
    CustomerSortingEnum SortBy) : IRequest<PaginatedResult<GetCustomerPaginatedListResponse>>;

