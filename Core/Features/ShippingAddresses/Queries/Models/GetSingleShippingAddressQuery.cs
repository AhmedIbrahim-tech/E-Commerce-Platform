using Core.Features.ShippingAddresses.Queries.Responses;

namespace Core.Features.ShippingAddresses.Queries.Models
{
    public record GetSingleShippingAddressQuery(Guid Id) : IRequest<ApiResponse<GetSingleShippingAddressResponse>>;
}
