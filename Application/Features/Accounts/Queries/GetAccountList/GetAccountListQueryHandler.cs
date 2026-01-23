using Application.Common.Bases;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Accounts.Queries.GetAccountList;

public class GetAccountListQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetAccountListQuery, ApiResponse<List<GetAccountListResponse>>>
{
    public async Task<ApiResponse<List<GetAccountListResponse>>> Handle(GetAccountListQuery request, CancellationToken cancellationToken)
    {
        var accountList = await unitOfWork.Accounts.GetTableNoTracking()
            .Select(a => new GetAccountListResponse(
                a.Id,
                a.AccountName,
                a.AccountNumber,
                a.BankName,
                a.BranchName,
                a.Iban,
                a.SwiftCode,
                a.InitialBalance,
                a.CurrentBalance,
                a.IsActive,
                a.CreatedTime
            ))
            .ToListAsync(cancellationToken);

        return Success(accountList);
    }
}
