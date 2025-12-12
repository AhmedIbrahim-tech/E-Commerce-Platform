using Application.Features.Employees.Commands.AddEmployee;
using Application.Features.Employees.Commands.EditEmployee;
using Application.Features.Employees.Commands.DeleteEmployee;
using Application.Features.Employees.Queries.GetEmployeeById;
using Application.Features.Employees.Queries.GetEmployeePaginatedList;
using API.Controllers.Base;

namespace API.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class EmployeeController : AppControllerBase
    {
        [Authorize(Roles = "Admin,Employee", Policy = "GetEmployee")]
        [HttpGet(Router.EmployeeRouting.GetById)]
        public async Task<IActionResult> GetEmployeeById([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new GetEmployeeByIdQuery(id)));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet(Router.EmployeeRouting.Paginated)]
        public async Task<IActionResult> GetEmployeePaginatedList([FromQuery] GetEmployeePaginatedListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(Router.EmployeeRouting.Create)]
        public async Task<IActionResult> CreateEmployee([FromBody] AddEmployeeCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin, Employee", Policy = "EditEmployee")]
        [HttpPut(Router.EmployeeRouting.Edit)]
        public async Task<IActionResult> EditEmployee([FromBody] EditEmployeeCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete(Router.EmployeeRouting.Delete)]
        public async Task<IActionResult> DeleteEmployee([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new DeleteEmployeeCommand(id)));
        }
    }
}
