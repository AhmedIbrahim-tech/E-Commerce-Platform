using Application.Features.Warranties.Commands.AddWarranty;
using Application.Features.Warranties.Commands.DeleteWarranty;
using Application.Features.Warranties.Commands.EditWarranty;
using Application.Features.Warranties.Queries.GetWarrantyById;
using Application.Features.Warranties.Queries.GetWarrantyList;
using Application.Features.Warranties.Queries.GetWarrantyPaginatedList;
using Infrastructure.Data.Authorization;

namespace API.Controllers.Catalog;

[Authorize]
public class WarrantyController : AppControllerBase
{
    [AllowAnonymous]
    [HttpGet(Router.WarrantyRouting.GetAll)]
    public async Task<IActionResult> GetWarrantyList()
    {
        var response = await Mediator.Send(new GetWarrantyListQuery());
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet(Router.WarrantyRouting.Paginated)]
    public async Task<IActionResult> GetWarrantyPaginatedList([FromQuery] GetWarrantyPaginatedListQuery query)
    {
        var response = await Mediator.Send(query);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost(Router.WarrantyRouting.GetById)]
    public async Task<IActionResult> GetWarrantyById([FromBody] GetWarrantyByIdQuery query)
    {
        return NewResult(await Mediator.Send(query));
    }

    [Authorize(Roles = Roles.Admin, Policy = Policies.Warranty.Create)]
    [HttpPost(Router.WarrantyRouting.Create)]
    public async Task<IActionResult> CreateWarranty([FromBody] AddWarrantyCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    [Authorize(Roles = Roles.Admin, Policy = Policies.Warranty.Edit)]
    [HttpPut(Router.WarrantyRouting.Edit)]
    public async Task<IActionResult> EditWarranty([FromBody] EditWarrantyCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    [Authorize(Roles = Roles.Admin, Policy = Policies.Warranty.Delete)]
    [HttpPost(Router.WarrantyRouting.Delete)]
    public async Task<IActionResult> DeleteWarranty([FromBody] DeleteWarrantyCommand command)
    {
        return NewResult(await Mediator.Send(command));
    }
}
