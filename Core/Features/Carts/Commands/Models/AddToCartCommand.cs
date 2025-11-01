
namespace Core.Features.Carts.Commands.Models
{
    public record AddToCartCommand(Guid ProductId, int Quantity) : IRequest<ApiResponse<string>>;
}
