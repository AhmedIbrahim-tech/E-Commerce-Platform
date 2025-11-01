
namespace Core.Features.Carts.Commands.Models
{
    public record DeleteCartCommand(Guid CartId) : IRequest<ApiResponse<string>>;
}
