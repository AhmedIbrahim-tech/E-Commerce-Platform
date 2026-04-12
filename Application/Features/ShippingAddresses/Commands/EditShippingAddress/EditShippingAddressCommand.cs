using Application.Common.Bases;

namespace Application.Features.ShippingAddresses.Commands.EditShippingAddress;

public record EditShippingAddressCommand
(
    Guid Id,
    string FullName,
    string Street,
    string City,
    string State
) : IRequest<ApiResponse<string>>;
