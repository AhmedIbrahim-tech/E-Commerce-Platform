using Application.Common.Bases;
using Application.Features.ApplicationUser.Commands.AddVendor;
using Application.Features.Vendors.Commands.DeleteVendor;
using Application.Features.Vendors.Commands.EditVendor;
using Application.Features.Vendors.Commands.ToggleVendorStatus;
using Application.Features.Vendors.Queries.GetVendorById;
using Application.Features.Vendors.Queries.GetVendorPaginatedList;
using Infrastructure.Data.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Users;

/// <summary>Vendor profile CRUD. Toggle activates/restores profile soft-delete (not the same as account lockout).</summary>
[Authorize(Roles = Roles.Admin + "," + Roles.SuperAdmin)]
public class VendorController : AppControllerBase
{
    [Authorize(Policy = Policies.Vendor.ViewList)]
    [HttpPost(Router.VendorRouting.Paginated)]
    public async Task<IActionResult> GetVendorPaginatedList([FromBody] GetVendorPaginatedListQuery query)
    {
        var response = await Mediator.Send(query);
        return NewResult(ApiResponseHandler.Success(response));
    }

    [Authorize(Policy = Policies.Vendor.ViewProfile)]
    [HttpPost(Router.VendorRouting.Prefix + "getById")]
    public async Task<IActionResult> GetVendorById([FromBody] GetVendorByIdQuery query)
    {
        return NewResult(await Mediator.Send(query));
    }

    /// <summary>Create vendor; multipart when sending <c>ProfileImage</c> (optional).</summary>
    [Authorize(Policy = Policies.Vendor.Create)]
    [HttpPost(Router.VendorRouting.Create)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateVendor([FromForm] AddVendorCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    /// <summary>Update vendor; optional avatar via multipart.</summary>
    [Authorize(Policy = Policies.Vendor.EditProfile)]
    [HttpPut(Router.VendorRouting.Edit)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> EditVendor([FromForm] EditVendorCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    [Authorize(Policy = Policies.Vendor.Delete)]
    [HttpDelete(Router.VendorRouting.Delete)]
    public async Task<IActionResult> DeleteVendor([FromRoute] Guid id)
    {
        return NewResult(await Mediator.Send(new DeleteVendorCommand(id)));
    }

    [Authorize(Policy = Policies.Vendor.EditProfile)]
    [HttpPost(Router.VendorRouting.ToggleStatus)]
    public async Task<IActionResult> ToggleVendorStatus([FromRoute] Guid id)
    {
        return NewResult(await Mediator.Send(new ToggleVendorStatusCommand(id)));
    }
}
