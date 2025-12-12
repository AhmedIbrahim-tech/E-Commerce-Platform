using Application.Common.Bases;

namespace Application.Features.ShippingAddresses.Commands.SetShippingAddress;

public record SetShippingAddressCommand(Guid OrderId, Guid ShippingAddressId) : IRequest<ApiResponse<string>>;

