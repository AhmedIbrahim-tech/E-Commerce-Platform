using Core.Features.ApplicationUser.Commands.Models;

namespace Core.Mapping.Customers
{
    public partial class CustomerProfile
    {
        public void AddCustomerCommandMapping()
        {
            CreateMap<AddCustomerCommand, Customer>();
        }
    }
}
