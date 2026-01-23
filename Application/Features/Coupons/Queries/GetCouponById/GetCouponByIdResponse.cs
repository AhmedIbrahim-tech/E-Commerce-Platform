namespace Application.Features.Coupons.Queries.GetCouponById;

public record GetCouponByIdResponse(Guid Id, string Code, string Name, string? Description, decimal DiscountAmount,
    decimal? DiscountPercentage, decimal? MinimumPurchaseAmount, decimal? MaximumDiscountAmount, 
    DateTimeOffset StartDate, DateTimeOffset EndDate, int? UsageLimit, int UsedCount, bool IsActive, DateTimeOffset CreatedTime);
