using Application.Common.Bases;

namespace Application.Features.Discounts.Queries.GetDiscountById;

public record GetDiscountByIdQuery(Guid Id) : IRequest<ApiResponse<GetDiscountByIdResponse>>;
