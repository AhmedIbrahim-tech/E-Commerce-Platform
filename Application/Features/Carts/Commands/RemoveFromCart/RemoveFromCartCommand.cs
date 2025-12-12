using Application.Common.Bases;

namespace Application.Features.Carts.Commands.RemoveFromCart;

public record RemoveFromCartCommand(Guid ProductId) : IRequest<ApiResponse<string>>;

