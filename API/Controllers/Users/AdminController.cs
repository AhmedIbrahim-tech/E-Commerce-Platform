using Application.Features.Admins.Commands.DeleteAdmin;
using Application.Features.Admins.Commands.ToggleAdminStatus;
using Application.Features.Admins.Commands.EditAdmin;
using Application.Features.Admins.Queries.GetAdminById;
using Application.Features.Admins.Queries.GetAdminPaginatedList;
using Application.Features.ApplicationUser.Commands.AddAdmin;
using Infrastructure.Data.Authorization;

namespace API.Controllers.Users;

[Authorize(Roles = Roles.Admin)]
public class AdminController : AppControllerBase
{
    [Authorize(Policy = Policies.Admin.ViewList)]
    [HttpPost(Router.AdminRouting.Paginated)]
    public async Task<IActionResult> GetAdminPaginatedList([FromBody] GetAdminPaginatedListQuery query)
    {
        var response = await Mediator.Send(query);
        return Ok(response);
    }

    [Authorize(Policy = Policies.Admin.ViewProfile)]
    [HttpPost(Router.AdminRouting.Prefix + "getById")]
    public async Task<IActionResult> GetAdminById([FromBody] GetAdminByIdQuery query)
    {
        return NewResult(await Mediator.Send(query));
    }

    [Authorize(Policy = Policies.Admin.Create)]
    [HttpPost(Router.AdminRouting.Create)]
    public async Task<IActionResult> CreateAdmin([FromBody] AddAdminCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    [Authorize(Policy = Policies.Admin.EditProfile)]
    [HttpPut(Router.AdminRouting.Edit)]
    public async Task<IActionResult> EditAdmin([FromForm] EditAdminCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    [Authorize(Policy = Policies.Admin.Delete)]
    [HttpDelete(Router.AdminRouting.Delete)]
    public async Task<IActionResult> DeleteAdmin([FromRoute] Guid id)
    {
        return NewResult(await Mediator.Send(new DeleteAdminCommand(id)));
    }

    [Authorize(Policy = Policies.Admin.EditProfile)]
    [HttpPost(Router.AdminRouting.ToggleStatus)]
    public async Task<IActionResult> ToggleAdminStatus([FromRoute] Guid id)
    {
        return NewResult(await Mediator.Send(new ToggleAdminStatusCommand(id)));
    }
}
