using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Accounts.Queries.GetAccountById;

public class GetAccountByIdQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetAccountByIdQuery, ApiResponse<GetAccountByIdResponse>>
{
    public async Task<ApiResponse<GetAccountByIdResponse>> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var account = await unitOfWork.Accounts.GetTableNoTracking()
            .Where(a => a.Id == request.Id)
            .Select(a => new GetAccountByIdResponse(
                a.Id,
                a.AccountName,
                a.AccountNumber,
                a.BankName,
                a.BranchName,
                a.Iban,
                a.SwiftCode,
                a.InitialBalance,
                a.CurrentBalance,
                a.Description,
                a.IsActive,
                a.CreatedTime
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (account == null)
            return NotFound<GetAccountByIdResponse>("Account not found");

        return Success(account);
    }
}
