using Application.Common.Bases;

namespace Application.Features.Carts.Commands.AddToCart;

public record AddToCartCommand(Guid ProductId, int Quantity) : IRequest<ApiResponse<string>>;

