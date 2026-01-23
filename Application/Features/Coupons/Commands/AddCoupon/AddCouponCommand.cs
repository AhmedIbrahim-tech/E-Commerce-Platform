using Application.Common.Bases;

namespace Application.Features.Coupons.Commands.AddCoupon;

public record AddCouponCommand(string Code, string Name, string? Description, decimal DiscountAmount, decimal? DiscountPercentage, 
    decimal? MinimumPurchaseAmount, decimal? MaximumDiscountAmount, DateTimeOffset StartDate, DateTimeOffset EndDate, 
    int? UsageLimit, bool IsActive) : IRequest<ApiResponse<string>>;
