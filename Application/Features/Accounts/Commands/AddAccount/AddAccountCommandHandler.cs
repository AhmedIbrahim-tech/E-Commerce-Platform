namespace Application.Features.Accounts.Commands.AddAccount;

public class AddAccountCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<AddAccountCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(AddAccountCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var account = new Account
            {
                AccountName = request.AccountName,
                AccountNumber = request.AccountNumber,
                BankName = request.BankName,
                BranchName = request.BranchName,
                Iban = request.Iban,
                SwiftCode = request.SwiftCode,
                InitialBalance = request.InitialBalance,
                CurrentBalance = request.InitialBalance,
                Description = request.Description,
                IsActive = request.IsActive
            };

            await unitOfWork.Accounts.AddAsync(account, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Created("");
        }
        catch (Exception)
        {
            return new ApiResponse<string>(AccountErrors.DuplicatedAccountNumber());
        }
    }
}
