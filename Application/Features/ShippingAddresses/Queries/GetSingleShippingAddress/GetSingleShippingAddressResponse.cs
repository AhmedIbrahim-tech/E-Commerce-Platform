namespace Application.Features.ShippingAddresses.Queries.GetSingleShippingAddress;

public record GetSingleShippingAddressResponse(Guid Id, string FullName, string Street, string City, string State);
