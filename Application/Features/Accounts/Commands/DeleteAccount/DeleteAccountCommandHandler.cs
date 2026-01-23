using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Accounts.Commands.DeleteAccount;

public class DeleteAccountCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<DeleteAccountCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await unitOfWork.Accounts.GetTableNoTracking()
            .Where(a => a.Id.Equals(request.Id))
            .FirstOrDefaultAsync(cancellationToken);

        if (account == null) return new ApiResponse<string>(AccountErrors.AccountNotFound());

        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await unitOfWork.Accounts.DeleteAsync(account, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return Deleted<string>();
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return BadRequest<string>("Cannot delete account");
        }
    }
}
