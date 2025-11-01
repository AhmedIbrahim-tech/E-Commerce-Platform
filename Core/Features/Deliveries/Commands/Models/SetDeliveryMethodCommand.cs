
namespace Core.Features.Deliveries.Commands.Models
{
    public record SetDeliveryMethodCommand(Guid OrderId, DeliveryMethod DeliveryMethod) : IRequest<ApiResponse<string>>;
}
