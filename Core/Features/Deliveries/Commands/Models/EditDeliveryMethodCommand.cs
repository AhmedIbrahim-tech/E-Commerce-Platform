
namespace Core.Features.Deliveries.Commands.Models
{
    public record EditDeliveryMethodCommand(Guid OrderId, DeliveryMethod DeliveryMethod) : IRequest<ApiResponse<string>>;
}
