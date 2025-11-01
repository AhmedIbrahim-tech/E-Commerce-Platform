using Core.Features.ShippingAddresses.Queries.Responses;

namespace Core.Mapping.ShippingAddresses
{
    public partial class ShippingAddressProfile
    {
        public void GetSingleShippingAddressQueryMapping()
        {
            CreateMap<ShippingAddress, GetSingleShippingAddressResponse>();
        }
    }
}
