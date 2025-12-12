namespace Application.Features.ShippingAddresses.Queries.GetSingleShippingAddress;

public record GetSingleShippingAddressResponse(Guid Id, string? FirstName, string? LastName, string? Street, string? City, string? State);

