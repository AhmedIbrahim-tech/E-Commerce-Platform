using Application.Common.Bases;

namespace Application.Features.ShippingAddresses.Queries.GetShippingAddressList;

public record GetShippingAddressListQuery : IRequest<ApiResponse<List<GetShippingAddressListResponse>>>;

