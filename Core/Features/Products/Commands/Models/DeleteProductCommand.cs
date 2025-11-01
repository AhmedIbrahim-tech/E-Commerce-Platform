
namespace Core.Features.Products.Commands.Models
{
    public record DeleteProductCommand(Guid ProductId) : IRequest<ApiResponse<string>>;
}
