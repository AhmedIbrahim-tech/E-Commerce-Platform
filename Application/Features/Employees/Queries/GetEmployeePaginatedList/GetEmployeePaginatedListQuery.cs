namespace Application.Features.Employees.Queries.GetEmployeePaginatedList;

public record GetEmployeePaginatedListQuery(int PageNumber, int PageSize, string? Search,
    EmployeeSortingEnum SortBy) : IRequest<PaginatedResult<GetEmployeePaginatedListResponse>>;

