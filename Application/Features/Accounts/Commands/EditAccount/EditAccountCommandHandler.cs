using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Accounts.Commands.EditAccount;

public class EditAccountCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<EditAccountCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(EditAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await unitOfWork.Accounts.GetTableNoTracking()
            .Where(a => a.Id.Equals(request.Id))
            .FirstOrDefaultAsync(cancellationToken);

        if (account == null) return new ApiResponse<string>(AccountErrors.AccountNotFound());

        account.AccountName = request.AccountName;
        account.AccountNumber = request.AccountNumber;
        account.BankName = request.BankName;
        account.BranchName = request.BranchName;
        account.Iban = request.Iban;
        account.SwiftCode = request.SwiftCode;
        account.InitialBalance = request.InitialBalance;
        account.Description = request.Description;
        account.IsActive = request.IsActive;

        try
        {
            await unitOfWork.Accounts.UpdateAsync(account, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Edit("");
        }
        catch (Exception)
        {
            return new ApiResponse<string>(AccountErrors.DuplicatedAccountNumber());
        }
    }
}
