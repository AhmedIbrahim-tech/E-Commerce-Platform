using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Coupons.Commands.DeleteCoupon;

public class DeleteCouponCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<DeleteCouponCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(DeleteCouponCommand request, CancellationToken cancellationToken)
    {
        var coupon = await unitOfWork.Coupons.GetTableNoTracking()
            .Where(c => c.Id.Equals(request.Id))
            .FirstOrDefaultAsync(cancellationToken);

        if (coupon == null) return new ApiResponse<string>(CouponErrors.CouponNotFound());

        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await unitOfWork.Coupons.DeleteAsync(coupon, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return Deleted<string>();
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return BadRequest<string>("Cannot delete coupon");
        }
    }
}
