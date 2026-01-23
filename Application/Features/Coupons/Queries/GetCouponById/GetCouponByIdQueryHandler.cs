using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Coupons.Queries.GetCouponById;

public class GetCouponByIdQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetCouponByIdQuery, ApiResponse<GetCouponByIdResponse>>
{
    public async Task<ApiResponse<GetCouponByIdResponse>> Handle(GetCouponByIdQuery request, CancellationToken cancellationToken)
    {
        var coupon = await unitOfWork.Coupons.GetTableNoTracking()
            .Where(c => c.Id == request.Id)
            .Select(c => new GetCouponByIdResponse(
                c.Id,
                c.Code,
                c.Name,
                c.Description,
                c.DiscountAmount,
                c.DiscountPercentage,
                c.MinimumPurchaseAmount,
                c.MaximumDiscountAmount,
                c.StartDate,
                c.EndDate,
                c.UsageLimit,
                c.UsedCount,
                c.IsActive,
                c.CreatedTime
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (coupon == null)
            return NotFound<GetCouponByIdResponse>("Coupon not found");

        return Success(coupon);
    }
}
