namespace Application.Features.Accounts.Queries.GetAccountPaginatedList;

public record GetAccountPaginatedListQuery(int PageNumber, int PageSize, string? Search,
    AccountSortingEnum SortBy) : IRequest<PaginatedResult<GetAccountPaginatedListResponse>>;
