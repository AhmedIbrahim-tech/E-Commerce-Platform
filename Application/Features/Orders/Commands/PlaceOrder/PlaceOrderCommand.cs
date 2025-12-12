using Application.Common.Bases;

namespace Application.Features.Orders.Commands.PlaceOrder;

public record PlaceOrderCommand(Guid OrderId) : IRequest<ApiResponse<PaymentProcessResponse>>;

