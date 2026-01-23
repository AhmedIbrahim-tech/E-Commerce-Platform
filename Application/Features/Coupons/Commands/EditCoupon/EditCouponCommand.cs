using Application.Common.Bases;

namespace Application.Features.Coupons.Commands.EditCoupon;

public record EditCouponCommand(Guid Id, string Code, string Name, string? Description, decimal DiscountAmount, decimal? DiscountPercentage,
    decimal? MinimumPurchaseAmount, decimal? MaximumDiscountAmount, DateTimeOffset StartDate, DateTimeOffset EndDate,
    int? UsageLimit, bool IsActive) : IRequest<ApiResponse<string>>;
