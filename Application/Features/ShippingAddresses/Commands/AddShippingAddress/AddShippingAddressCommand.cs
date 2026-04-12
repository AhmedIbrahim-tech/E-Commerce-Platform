using Application.Common.Bases;

namespace Application.Features.ShippingAddresses.Commands.AddShippingAddress;

public record AddShippingAddressCommand
(
    string FullName,
    string Street,
    string City,
    string State
) : IRequest<ApiResponse<string>>;
