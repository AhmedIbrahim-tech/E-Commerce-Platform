using Application.Common.Bases;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Coupons.Queries.GetCouponList;

public class GetCouponListQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetCouponListQuery, ApiResponse<List<GetCouponListResponse>>>
{
    public async Task<ApiResponse<List<GetCouponListResponse>>> Handle(GetCouponListQuery request, CancellationToken cancellationToken)
    {
        var couponList = await unitOfWork.Coupons.GetTableNoTracking()
            .Select(c => new GetCouponListResponse(
                c.Id,
                c.Code,
                c.Name,
                c.Description,
                c.DiscountAmount,
                c.DiscountPercentage,
                c.StartDate,
                c.EndDate,
                c.UsageLimit,
                c.UsedCount,
                c.IsActive,
                c.CreatedTime
            ))
            .ToListAsync(cancellationToken);

        return Success(couponList);
    }
}
