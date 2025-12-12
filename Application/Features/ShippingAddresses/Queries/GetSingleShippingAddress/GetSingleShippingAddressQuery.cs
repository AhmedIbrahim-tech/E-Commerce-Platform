using Application.Common.Bases;

namespace Application.Features.ShippingAddresses.Queries.GetSingleShippingAddress;

public record GetSingleShippingAddressQuery(Guid Id) : IRequest<ApiResponse<GetSingleShippingAddressResponse>>;

