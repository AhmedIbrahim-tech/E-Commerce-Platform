using Application.Features.ApplicationUser.Commands.AddCustomer;
using Application.Features.Customers.Commands.EditCustomer;
using Application.Features.Customers.Commands.DeleteCustomer;
using Application.Features.Customers.Commands.ToggleCustomerStatus;
using Application.Features.Customers.Queries.GetCustomerById;
using Application.Features.Customers.Queries.GetCustomerPaginatedList;
using API.Controllers.Base;
using Infrastructure.Data.Authorization;

namespace API.Controllers.Users
{
    [Authorize(Roles = Roles.Admin)]
    public class CustomerController : AppControllerBase
    {
        [Authorize(Policy = Policies.Customer.ViewList)]
        [HttpPost(Router.CustomerRouting.Paginated)]
        public async Task<IActionResult> GetCustomerPaginatedList([FromBody] GetCustomerPaginatedListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [Authorize(Policy = Policies.Customer.ViewProfile)]
        [HttpPost(Router.CustomerRouting.Prefix + "getById")]
        public async Task<IActionResult> GetCustomerById([FromBody] GetCustomerByIdQuery query)
        {
            return NewResult(await Mediator.Send(query));
        }

        [Authorize(Policy = Policies.Admin.ManageUsers)]
        [HttpPost(Router.CustomerRouting.Create)]
        public async Task<IActionResult> CreateCustomer([FromBody] AddCustomerCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Policy = Policies.Customer.EditProfile)]
        [HttpPut(Router.CustomerRouting.Edit)]
        public async Task<IActionResult> EditCustomer([FromBody] EditCustomerCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Policy = Policies.Customer.Delete)]
        [HttpDelete(Router.CustomerRouting.Delete)]
        public async Task<IActionResult> DeleteCustomer([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new DeleteCustomerCommand(id)));
        }

        [Authorize(Policy = Policies.Customer.EditProfile)]
        [HttpPost(Router.CustomerRouting.ToggleStatus)]
        public async Task<IActionResult> ToggleCustomerStatus([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new ToggleCustomerStatusCommand(id)));
        }
    }
}
