namespace Application.Features.Coupons.Queries.GetCouponPaginatedList;

public record GetCouponPaginatedListResponse(Guid Id, string Code, string Name, string? Description, decimal DiscountAmount,
    decimal? DiscountPercentage, DateTimeOffset StartDate, DateTimeOffset EndDate, int? UsageLimit, int UsedCount,
    bool IsActive, DateTimeOffset CreatedTime);
