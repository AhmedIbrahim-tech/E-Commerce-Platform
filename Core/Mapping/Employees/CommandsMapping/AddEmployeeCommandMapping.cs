using Core.Features.Employees.Commands.Models;

namespace Core.Mapping.Employees
{
    public partial class EmployeeProfile
    {
        public void AddEmployeeCommandMapping()
        {
            CreateMap<AddEmployeeCommand, Employee>();
        }
    }
}
