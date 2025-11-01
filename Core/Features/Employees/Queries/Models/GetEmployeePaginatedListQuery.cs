using Core.Features.Employees.Queries.Responses;

namespace Core.Features.Employees.Queries.Models
{
    public record GetEmployeePaginatedListQuery(int PageNumber, int PageSize, string? Search,
    EmployeeSortingEnum SortBy) : IRequest<PaginatedResult<GetEmployeePaginatedListResponse>>;
}
