using Core.Features.Orders.Commands.Responses;

namespace Core.Features.Orders.Commands.Models
{
    public record PlaceOrderCommand(Guid OrderId) : IRequest<ApiResponse<PaymentProcessResponse>>;
}
