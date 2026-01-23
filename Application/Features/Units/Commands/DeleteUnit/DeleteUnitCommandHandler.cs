using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Units.Commands.DeleteUnit;

public class DeleteUnitCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<DeleteUnitCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(DeleteUnitCommand request, CancellationToken cancellationToken)
    {
        var unit = await unitOfWork.UnitOfMeasures.GetTableNoTracking()
            .Where(u => u.Id.Equals(request.Id))
            .FirstOrDefaultAsync(cancellationToken);

        if (unit == null) return new ApiResponse<string>(UnitOfMeasureErrors.UnitOfMeasureNotFound());

        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await unitOfWork.UnitOfMeasures.DeleteAsync(unit, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return Deleted<string>();
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return new ApiResponse<string>(UnitOfMeasureErrors.CannotDeleteUnitOfMeasureWithProducts());
        }
    }
}
