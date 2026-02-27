using Application.Features.ApplicationUser.Commands.AddCustomer;
using Application.Features.ApplicationUser.Commands.AddAdmin;
using Application.Features.ApplicationUser.Commands.AddVendor;
using Application.Features.ApplicationUser.Commands.ChangeUserPassword;
using Application.Features.ApplicationUser.Commands.EditMyProfile;
using Application.Features.ApplicationUser.Commands.ToggleUserStatus;
using Application.Features.ApplicationUser.Queries.GetMyProfile;
using Application.Features.ApplicationUser.Queries.GetUserById;
using Application.Features.ApplicationUser.Queries.GetUsersPaginatedList;
using Infrastructure.Data.Authorization;

namespace API.Controllers.Users;

[Authorize]
public class ApplicationUserController : AppControllerBase
{
    [AllowAnonymous]
    [HttpPost(Router.UserRouting.Register)]
    public async Task<IActionResult> Register([FromBody] AddCustomerCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    [Authorize(Policy = Policies.Auth.ChangePassword)]
    [HttpPut(Router.UserRouting.ChangePassword)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangeUserPasswordCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    [Authorize(Policy = Policies.Auth.ViewOwnProfile)]
    [HttpGet(Router.UserRouting.Profile)]
    public async Task<IActionResult> GetMyProfile()
    {
        var response = await Mediator.Send(new GetMyProfileQuery());
        return NewResult(response);
    }

    [Authorize(Policy = Policies.Auth.EditOwnProfile)]
    [HttpPut(Router.UserRouting.Profile)]
    public async Task<IActionResult> EditMyProfile([FromBody] EditMyProfileCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    [Authorize(Roles = Roles.Admin, Policy = Policies.Admin.Create)]
    [HttpPost(Router.UserRouting.CreateAdmin)]
    public async Task<IActionResult> CreateAdmin([FromBody] AddAdminCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    [Authorize(Roles = Roles.Admin, Policy = Policies.Vendor.Create)]
    [HttpPost(Router.UserRouting.CreateVendor)]
    public async Task<IActionResult> CreateVendor([FromBody] AddVendorCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    // Unified Users Management
    [Authorize(Roles = Roles.Admin, Policy = Policies.Admin.ViewList)]
    [HttpPost(Router.UserRouting.UsersPaginated)]
    public async Task<IActionResult> GetUsersPaginatedList([FromBody] GetUsersPaginatedListQuery query)
    {
        var response = await Mediator.Send(query);
        return Ok(response);
    }

    [Authorize(Roles = Roles.Admin, Policy = Policies.Admin.ViewProfile)]
    [HttpGet(Router.UserRouting.GetUserById)]
    public async Task<IActionResult> GetUserById([FromRoute] Guid id)
    {
        var response = await Mediator.Send(new GetUserByIdQuery(id));
        return NewResult(response);
    }

    [Authorize(Roles = Roles.Admin, Policy = Policies.Admin.EditProfile)]
    [HttpPut(Router.UserRouting.ToggleUserStatus)]
    public async Task<IActionResult> ToggleUserStatus([FromBody] ToggleUserStatusCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }
}
