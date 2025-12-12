using Application.Common.Bases;

namespace Application.Features.ShippingAddresses.Commands.AddShippingAddress;

public record AddShippingAddressCommand
(
    string? FirstName,
    string? LastName,
    string? Street,
    string? City,
    string? State
) : IRequest<ApiResponse<string>>;

