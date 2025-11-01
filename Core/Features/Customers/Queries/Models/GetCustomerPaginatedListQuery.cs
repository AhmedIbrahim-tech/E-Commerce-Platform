using Core.Features.Customers.Queries.Responses;

namespace Core.Features.Customers.Queries.Models
{
    public record GetCustomerPaginatedListQuery(int PageNumber, int PageSize, string? Search,
    CustomerSortingEnum SortBy) : IRequest<PaginatedResult<GetCustomerPaginatedListResponse>>;
}
