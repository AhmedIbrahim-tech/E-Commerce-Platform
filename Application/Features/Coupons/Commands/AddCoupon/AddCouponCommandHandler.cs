namespace Application.Features.Coupons.Commands.AddCoupon;

public class AddCouponCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<AddCouponCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(AddCouponCommand request, CancellationToken cancellationToken)
    {
        if (request.EndDate <= request.StartDate)
            return new ApiResponse<string>(CouponErrors.InvalidDateRange());

        try
        {
            var coupon = new Coupon
            {
                Code = request.Code,
                Name = request.Name,
                Description = request.Description,
                DiscountAmount = request.DiscountAmount,
                DiscountPercentage = request.DiscountPercentage,
                MinimumPurchaseAmount = request.MinimumPurchaseAmount,
                MaximumDiscountAmount = request.MaximumDiscountAmount,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                UsageLimit = request.UsageLimit,
                IsActive = request.IsActive
            };

            await unitOfWork.Coupons.AddAsync(coupon, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Created("");
        }
        catch (Exception)
        {
            return new ApiResponse<string>(CouponErrors.DuplicatedCouponCode());
        }
    }
}
