using Application.Common.Bases;

namespace Application.Features.Discounts.Commands.DeleteDiscount;

public record DeleteDiscountCommand(Guid Id) : IRequest<ApiResponse<string>>;
