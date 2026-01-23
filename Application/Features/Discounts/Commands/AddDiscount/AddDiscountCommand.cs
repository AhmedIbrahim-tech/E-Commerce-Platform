using Application.Common.Bases;

namespace Application.Features.Discounts.Commands.AddDiscount;

public record AddDiscountCommand(string Name, string? Description, decimal DiscountAmount, decimal? DiscountPercentage,
    DateTimeOffset StartDate, DateTimeOffset EndDate, Guid? DiscountPlanId, bool IsActive) : IRequest<ApiResponse<string>>;
