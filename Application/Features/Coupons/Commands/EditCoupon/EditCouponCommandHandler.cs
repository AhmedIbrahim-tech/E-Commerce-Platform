using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Coupons.Commands.EditCoupon;

public class EditCouponCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<EditCouponCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(EditCouponCommand request, CancellationToken cancellationToken)
    {
        if (request.EndDate <= request.StartDate)
            return new ApiResponse<string>(CouponErrors.InvalidDateRange());

        var coupon = await unitOfWork.Coupons.GetTableNoTracking()
            .Where(c => c.Id.Equals(request.Id))
            .FirstOrDefaultAsync(cancellationToken);

        if (coupon == null) return new ApiResponse<string>(CouponErrors.CouponNotFound());

        coupon.Code = request.Code;
        coupon.Name = request.Name;
        coupon.Description = request.Description;
        coupon.DiscountAmount = request.DiscountAmount;
        coupon.DiscountPercentage = request.DiscountPercentage;
        coupon.MinimumPurchaseAmount = request.MinimumPurchaseAmount;
        coupon.MaximumDiscountAmount = request.MaximumDiscountAmount;
        coupon.StartDate = request.StartDate;
        coupon.EndDate = request.EndDate;
        coupon.UsageLimit = request.UsageLimit;
        coupon.IsActive = request.IsActive;

        try
        {
            await unitOfWork.Coupons.UpdateAsync(coupon, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Edit("");
        }
        catch (Exception)
        {
            return new ApiResponse<string>(CouponErrors.DuplicatedCouponCode());
        }
    }
}
