using Application.Common.Bases;

namespace Application.Features.Deliveries.Commands.SetDeliveryMethod;

public record SetDeliveryMethodCommand(Guid OrderId, DeliveryMethod DeliveryMethod) : IRequest<ApiResponse<string>>;

