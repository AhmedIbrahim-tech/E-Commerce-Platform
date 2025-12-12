using Application.Common.Bases;

namespace Application.Features.Carts.Queries.GetCartById;

public record GetCartByIdQuery(Guid Id) : IRequest<ApiResponse<GetCartByIdResponse>>;

