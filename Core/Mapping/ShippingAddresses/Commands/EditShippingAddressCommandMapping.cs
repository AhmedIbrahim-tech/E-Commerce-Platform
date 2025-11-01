using Core.Features.ShippingAddresses.Commands.Models;

namespace Core.Mapping.ShippingAddresses
{
    public partial class ShippingAddressProfile
    {
        public void EditShippingAddressCommandMapping()
        {
            CreateMap<EditShippingAddressCommand, ShippingAddress>();
        }
    }
}
