namespace Application.Features.ShippingAddresses.Queries.GetShippingAddressList;

public record GetShippingAddressListResponse(Guid Id, string FullName, string Street, string City, string State);
