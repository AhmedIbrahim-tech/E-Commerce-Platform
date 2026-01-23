using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Discounts.Commands.DeleteDiscount;

public class DeleteDiscountCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<DeleteDiscountCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
    {
        var discount = await unitOfWork.Discounts.GetTableNoTracking()
            .Where(d => d.Id.Equals(request.Id))
            .FirstOrDefaultAsync(cancellationToken);

        if (discount == null) return new ApiResponse<string>(DiscountErrors.DiscountNotFound());

        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await unitOfWork.Discounts.DeleteAsync(discount, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return Deleted<string>();
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return BadRequest<string>("Cannot delete discount");
        }
    }
}
