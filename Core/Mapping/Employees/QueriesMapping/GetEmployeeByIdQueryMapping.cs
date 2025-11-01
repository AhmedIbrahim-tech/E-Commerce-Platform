using Core.Features.Employees.Queries.Responses;

namespace Core.Mapping.Employees
{
    public partial class EmployeeProfile
    {
        public void GetEmployeeByIdQueryMapping()
        {
            CreateMap<Employee, GetSingleEmployeeResponse>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));
        }
    }
}
