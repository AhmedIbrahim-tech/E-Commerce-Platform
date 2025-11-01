using Core.Features.ShippingAddresses.Queries.Responses;

namespace Core.Mapping.ShippingAddresses
{
    public partial class ShippingAddressProfile
    {
        public void GetShippingAddressListQueryMapping()
        {
            CreateMap<ShippingAddress, GetShippingAddressListResponse>();
        }
    }
}
