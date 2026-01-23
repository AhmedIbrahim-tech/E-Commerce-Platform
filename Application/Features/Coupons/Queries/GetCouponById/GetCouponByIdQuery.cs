using Application.Common.Bases;

namespace Application.Features.Coupons.Queries.GetCouponById;

public record GetCouponByIdQuery(Guid Id) : IRequest<ApiResponse<GetCouponByIdResponse>>;
