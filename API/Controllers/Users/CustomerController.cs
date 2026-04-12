using Application.Common.Bases;
using Application.Features.ApplicationUser.Commands.AddCustomer;
using Application.Features.Customers.Commands.DeleteCustomer;
using Application.Features.Customers.Commands.EditCustomer;
using Application.Features.Customers.Commands.ToggleCustomerStatus;
using Application.Features.Customers.Queries.GetCustomerById;
using Application.Features.Customers.Queries.GetCustomerPaginatedList;
using Infrastructure.Data.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Users;

/// <summary>Customer profile management for operators.</summary>
[Authorize(Roles = Roles.Admin + "," + Roles.SuperAdmin)]
public class CustomerController : AppControllerBase
{
    [Authorize(Policy = Policies.Customer.ViewList)]
    [HttpPost(Router.CustomerRouting.Paginated)]
    public async Task<IActionResult> GetCustomerPaginatedList([FromBody] GetCustomerPaginatedListQuery query)
    {
        var response = await Mediator.Send(query);
        return NewResult(ApiResponseHandler.Success(response));
    }

    [Authorize(Policy = Policies.Customer.ViewProfile)]
    [HttpPost(Router.CustomerRouting.Prefix + "getById")]
    public async Task<IActionResult> GetCustomerById([FromBody] GetCustomerByIdQuery query)
    {
        return NewResult(await Mediator.Send(query));
    }

    /// <summary>Create customer with optional avatar; use <c>multipart/form-data</c>.</summary>
    [Authorize(Policy = Policies.Admin.ManageUsers)]
    [HttpPost(Router.CustomerRouting.Create)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateCustomer([FromForm] AddCustomerCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    /// <summary>Update customer; optional avatar via multipart.</summary>
    [Authorize(Policy = Policies.Customer.EditProfile)]
    [HttpPut(Router.CustomerRouting.Edit)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> EditCustomer([FromForm] EditCustomerCommand command)
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
