using Application.Common.Bases;

namespace Application.Features.Carts.Commands.DeleteCart;

public record DeleteCartCommand(Guid CartId) : IRequest<ApiResponse<string>>;

