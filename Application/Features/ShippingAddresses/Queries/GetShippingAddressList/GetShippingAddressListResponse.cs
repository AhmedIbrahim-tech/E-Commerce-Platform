namespace Application.Features.ShippingAddresses.Queries.GetShippingAddressList;

public record GetShippingAddressListResponse(Guid Id, string FirstName, string LastName, string Street, string City, string State);

