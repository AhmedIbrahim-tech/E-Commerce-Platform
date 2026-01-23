using Application.Common.Bases;
using Domain.Entities.Accounts;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Accounts.Queries.GetAccountPaginatedList;

public class GetAccountPaginatedListQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetAccountPaginatedListQuery, PaginatedResult<GetAccountPaginatedListResponse>>
{
    public async Task<PaginatedResult<GetAccountPaginatedListResponse>> Handle(GetAccountPaginatedListQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Account, GetAccountPaginatedListResponse>> expression = a => new GetAccountPaginatedListResponse(
            a.Id,
            a.AccountName,
            a.AccountNumber,
            a.BankName,
            a.BranchName,
            a.InitialBalance,
            a.CurrentBalance,
            a.IsActive,
            a.CreatedTime
        );

        var queryable = unitOfWork.Accounts.GetTableNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
            queryable = queryable.Where(a => a.AccountName.Contains(request.Search!) || 
                a.AccountNumber.Contains(request.Search!) ||
                (a.BankName != null && a.BankName.Contains(request.Search!)));

        queryable = request.SortBy switch
        {
            AccountSortingEnum.AccountNameAsc => queryable.OrderBy(a => a.AccountName),
            AccountSortingEnum.AccountNameDesc => queryable.OrderByDescending(a => a.AccountName),
            AccountSortingEnum.CreatedTimeAsc => queryable.OrderBy(a => a.CreatedTime),
            AccountSortingEnum.CreatedTimeDesc => queryable.OrderByDescending(a => a.CreatedTime),
            _ => queryable.OrderBy(a => a.AccountName)
        };

        var paginatedList = await queryable.Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize);
        paginatedList.Meta = new { Count = paginatedList.Data.Count() };
        return paginatedList;
    }
}
