using Application.Common.Bases;

namespace Application.Features.Deliveries.Commands.EditDeliveryMethod;

public record EditDeliveryMethodCommand(Guid OrderId, DeliveryMethod DeliveryMethod) : IRequest<ApiResponse<string>>;

