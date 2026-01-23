namespace Application.Features.Discounts.Queries.GetDiscountById;

public record GetDiscountByIdResponse(Guid Id, string Name, string? Description, decimal DiscountAmount,
    decimal? DiscountPercentage, DateTimeOffset StartDate, DateTimeOffset EndDate, Guid? DiscountPlanId,
    string? DiscountPlanName, bool IsActive, DateTimeOffset CreatedTime);
