using Core.Features.ShippingAddresses.Queries.Responses;

namespace Core.Features.ShippingAddresses.Queries.Models
{
    public record GetShippingAddressListQuery : IRequest<ApiResponse<List<GetShippingAddressListResponse>>>;
}
