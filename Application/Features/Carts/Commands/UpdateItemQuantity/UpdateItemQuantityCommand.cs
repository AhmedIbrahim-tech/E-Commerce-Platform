using Application.Common.Bases;

namespace Application.Features.Carts.Commands.UpdateItemQuantity;

public record UpdateItemQuantityCommand(Guid ProductId, int Quantity) : IRequest<ApiResponse<string>>;

