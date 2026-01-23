namespace Application.Features.Coupons.Queries.GetCouponList;

public record GetCouponListResponse(Guid Id, string Code, string Name, string? Description, decimal DiscountAmount, 
    decimal? DiscountPercentage, DateTimeOffset StartDate, DateTimeOffset EndDate, int? UsageLimit, int UsedCount, 
    bool IsActive, DateTimeOffset CreatedTime);
