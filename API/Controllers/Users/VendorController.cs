using Application.Features.ApplicationUser.Commands.AddVendor;
using Application.Features.Vendors.Commands.EditVendor;
using Application.Features.Vendors.Commands.DeleteVendor;
using Application.Features.Vendors.Commands.ToggleVendorStatus;
using Application.Features.Vendors.Queries.GetVendorById;
using Application.Features.Vendors.Queries.GetVendorPaginatedList;
using API.Controllers.Base;
using Infrastructure.Data.Authorization;

namespace API.Controllers.Users
{
    [Authorize(Roles = Roles.Admin)]
    public class VendorController : AppControllerBase
    {
        [Authorize(Policy = Policies.Vendor.ViewList)]
        [HttpPost(Router.VendorRouting.Paginated)]
        public async Task<IActionResult> GetVendorPaginatedList([FromBody] GetVendorPaginatedListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [Authorize(Policy = Policies.Vendor.ViewProfile)]
        [HttpPost(Router.VendorRouting.Prefix + "getById")]
        public async Task<IActionResult> GetVendorById([FromBody] GetVendorByIdQuery query)
        {
            return NewResult(await Mediator.Send(query));
        }

        [Authorize(Policy = Policies.Vendor.Create)]
        [HttpPost(Router.VendorRouting.Create)]
        public async Task<IActionResult> CreateVendor([FromBody] AddVendorCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Policy = Policies.Vendor.EditProfile)]
        [HttpPut(Router.VendorRouting.Edit)]
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
}
