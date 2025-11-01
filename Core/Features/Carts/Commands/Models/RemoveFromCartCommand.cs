
namespace Core.Features.Carts.Commands.Models
{
    public record RemoveFromCartCommand(Guid ProductId) : IRequest<ApiResponse<string>>;
}
