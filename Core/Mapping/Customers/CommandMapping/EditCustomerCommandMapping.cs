using Core.Features.Customers.Commands.Models;

namespace Core.Mapping.Customers
{
    public partial class CustomerProfile
    {
        public void EditCustomerCommandMapping()
        {
            CreateMap<EditCustomerCommand, Customer>();
        }
    }
}
