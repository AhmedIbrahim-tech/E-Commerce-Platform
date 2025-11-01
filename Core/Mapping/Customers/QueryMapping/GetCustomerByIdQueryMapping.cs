using Core.Features.Customers.Queries.Responses;

namespace Core.Mapping.Customers
{
    public partial class CustomerProfile
    {
        public void GetCustomerByIdQueryMapping()
        {
            CreateMap<Customer, GetSingleCustomerResponse>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));
        }
    }
}
