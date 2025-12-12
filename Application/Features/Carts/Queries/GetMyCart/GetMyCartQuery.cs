using Application.Common.Bases;

namespace Application.Features.Carts.Queries.GetMyCart;

public record GetMyCartQuery : IRequest<ApiResponse<GetMyCartResponse>>;

