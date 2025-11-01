
namespace Core.Features.Orders.Commands.Models
{
    public record DeleteOrderCommand(Guid OrderId) : IRequest<ApiResponse<string>>;
}
