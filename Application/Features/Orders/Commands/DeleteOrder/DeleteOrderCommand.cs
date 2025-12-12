using Application.Common.Bases;

namespace Application.Features.Orders.Commands.DeleteOrder;

public record DeleteOrderCommand(Guid OrderId) : IRequest<ApiResponse<string>>;

