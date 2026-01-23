using Application.Common.Bases;

namespace Application.Features.Coupons.Queries.GetCouponList;

public record GetCouponListQuery() : IRequest<ApiResponse<List<GetCouponListResponse>>>;
