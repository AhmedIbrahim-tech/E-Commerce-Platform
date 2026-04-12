using Application.Common.Bases;
using Application.Features.ApplicationUser.Commands.AddAdmin;
using Application.Features.ApplicationUser.Commands.AddVendor;
using Application.Features.ApplicationUser.Commands.EditMyProfile;
using Application.Features.ApplicationUser.Commands.ToggleUserStatus;
using Application.Features.ApplicationUser.Queries.GetMyProfile;
using Application.Features.ApplicationUser.Queries.GetUserById;
using Application.Features.ApplicationUser.Queries.GetUsersPaginatedList;
using Infrastructure.Data.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Users;

[Authorize]
public class ApplicationUserController : AppControllerBase
{
    [Authorize(Policy = Policies.Auth.ViewOwnProfile)]
    [HttpGet(Router.UserRouting.Profile)]
    public async Task<IActionResult> GetMyProfile()
    {
        var response = await Mediator.Send(new GetMyProfileQuery());
        return NewResult(response);
    }

    /// <summary>Current user profile; optional avatar requires multipart (not JSON).</summary>
    [Authorize(Policy = Policies.Auth.EditOwnProfile)]
    [HttpPut(Router.UserRouting.Profile)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> EditMyProfile([FromForm] EditMyProfileCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    /// <summary>Alternate create-admin route; same idea as <c>POST v1/admin/create</c> (multipart if avatar).</summary>
    [Authorize(Roles = Roles.Admin + "," + Roles.SuperAdmin, Policy = Policies.Admin.Create)]
    [HttpPost(Router.UserRouting.CreateAdmin)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateAdmin([FromForm] AddAdminCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    /// <summary>Alternate create-vendor route; multipart when sending a profile image file.</summary>
    [Authorize(Roles = Roles.Admin + "," + Roles.SuperAdmin, Policy = Policies.Vendor.Create)]
    [HttpPost(Router.UserRouting.CreateVendor)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateVendor([FromForm] AddVendorCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    // Unified Users Management
    [Authorize(Roles = Roles.Admin + "," + Roles.SuperAdmin, Policy = Policies.Admin.ViewList)]
    [HttpPost(Router.UserRouting.UsersPaginated)]
    public async Task<IActionResult> GetUsersPaginatedList([FromBody] GetUsersPaginatedListQuery query)
    {
        var response = await Mediator.Send(query);
        return NewResult(ApiResponseHandler.Success(response));
    }

    [Authorize(Roles = Roles.Admin + "," + Roles.SuperAdmin, Policy = Policies.Admin.ViewProfile)]
    [HttpGet(Router.UserRouting.GetUserById)]
    public async Task<IActionResult> GetUserById([FromRoute] Guid id)
    {
        var response = await Mediator.Send(new GetUserByIdQuery(id));
        return NewResult(response);
    }

    /// <summary>AppUser lockout toggle; JSON body only (no file).</summary>
    [Authorize(Roles = Roles.Admin + "," + Roles.SuperAdmin, Policy = Policies.Admin.EditProfile)]
    [HttpPut(Router.UserRouting.ToggleUserStatus)]
    [Consumes("application/json")]
    public async Task<IActionResult> ToggleUserStatus([FromBody] ToggleUserStatusCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }
}
