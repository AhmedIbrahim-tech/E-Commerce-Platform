
namespace Core.Features.Payments.Commands.Models
{
    public record SetPaymentMethodCommand(Guid OrderId, PaymentMethod PaymentMethod) : IRequest<ApiResponse<string>>;
}
