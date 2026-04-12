using Application.Common.Bases;
using Application.Features.Admins.Commands.DeleteAdmin;
using Application.Features.Admins.Commands.ToggleAdminStatus;
using Application.Features.Admins.Commands.EditAdmin;
using Application.Features.Admins.Queries.GetAdminById;
using Application.Features.Admins.Queries.GetAdminPaginatedList;
using Application.Features.ApplicationUser.Commands.AddAdmin;
using Infrastructure.Data.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Users;

/// <summary>Admin profile CRUD. List responses use the standard <see cref="ApiResponse{T}"/> envelope.</summary>
[Authorize(Roles = Roles.Admin + "," + Roles.SuperAdmin)]
public class AdminController : AppControllerBase
{
    [Authorize(Policy = Policies.Admin.ViewList)]
    [HttpPost(Router.AdminRouting.Paginated)]
    public async Task<IActionResult> GetAdminPaginatedList([FromBody] GetAdminPaginatedListQuery query)
    {
        var response = await Mediator.Send(query);
        return NewResult(ApiResponseHandler.Success(response));
    }

    [Authorize(Policy = Policies.Admin.ViewProfile)]
    [HttpPost(Router.AdminRouting.Prefix + "getById")]
    public async Task<IActionResult> GetAdminById([FromBody] GetAdminByIdQuery query)
    {
        return NewResult(await Mediator.Send(query));
    }

    /// <summary>Create admin with optional avatar. Use <c>multipart/form-data</c> (profile image is a file field).</summary>
    [Authorize(Policy = Policies.Admin.Create)]
    [HttpPost(Router.AdminRouting.Create)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateAdmin([FromForm] AddAdminCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    /// <summary>Update admin; optional new avatar via multipart.</summary>
    [Authorize(Policy = Policies.Admin.EditProfile)]
    [HttpPut(Router.AdminRouting.Edit)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> EditAdmin([FromForm] EditAdminCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    // No change required – DELETE by id uses route param as allowed by rule.
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
