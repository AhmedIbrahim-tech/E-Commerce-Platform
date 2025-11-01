
namespace Core.Features.Carts.Commands.Models
{
    public record UpdateItemQuantityCommand(Guid ProductId, int Quantity) : IRequest<ApiResponse<string>>;
}
