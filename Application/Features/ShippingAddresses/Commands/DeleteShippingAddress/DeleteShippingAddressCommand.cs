using Application.Common.Bases;

namespace Application.Features.ShippingAddresses.Commands.DeleteShippingAddress;

public record DeleteShippingAddressCommand(Guid Id) : IRequest<ApiResponse<string>>;

