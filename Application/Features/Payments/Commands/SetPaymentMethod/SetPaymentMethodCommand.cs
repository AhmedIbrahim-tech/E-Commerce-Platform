using Application.Common.Bases;

namespace Application.Features.Payments.Commands.SetPaymentMethod;

public record SetPaymentMethodCommand(Guid OrderId, PaymentMethod PaymentMethod) : IRequest<ApiResponse<string>>;

