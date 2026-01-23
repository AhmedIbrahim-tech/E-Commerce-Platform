using Application.Common.Bases;

namespace Application.Features.Discounts.Commands.EditDiscount;

public record EditDiscountCommand(Guid Id, string Name, string? Description, decimal DiscountAmount, decimal? DiscountPercentage,
    DateTimeOffset StartDate, DateTimeOffset EndDate, Guid? DiscountPlanId, bool IsActive) : IRequest<ApiResponse<string>>;
