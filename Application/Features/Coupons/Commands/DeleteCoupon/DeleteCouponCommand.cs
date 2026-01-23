using Application.Common.Bases;

namespace Application.Features.Coupons.Commands.DeleteCoupon;

public record DeleteCouponCommand(Guid Id) : IRequest<ApiResponse<string>>;
