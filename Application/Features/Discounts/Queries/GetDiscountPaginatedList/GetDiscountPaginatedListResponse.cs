namespace Application.Features.Discounts.Queries.GetDiscountPaginatedList;

public record GetDiscountPaginatedListResponse(Guid Id, string Name, string? Description, decimal DiscountAmount,
    decimal? DiscountPercentage, DateTimeOffset StartDate, DateTimeOffset EndDate, Guid? DiscountPlanId,
    string? DiscountPlanName, bool IsActive, DateTimeOffset CreatedTime);
