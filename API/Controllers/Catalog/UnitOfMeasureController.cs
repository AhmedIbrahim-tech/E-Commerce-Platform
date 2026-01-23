using Application.Features.Units.Commands.AddUnit;
using Application.Features.Units.Commands.DeleteUnit;
using Application.Features.Units.Commands.EditUnit;
using Application.Features.Units.Queries.GetUnitById;
using Application.Features.Units.Queries.GetUnitList;
using Application.Features.Units.Queries.GetUnitPaginatedList;
using Infrastructure.Data.Authorization;

namespace API.Controllers.Catalog
{
    [Authorize]
    public class UnitOfMeasureController : AppControllerBase
    {
        [AllowAnonymous]
        [HttpPost(Router.UnitOfMeasureRouting.GetAll)]
        public async Task<IActionResult> GetUnitList()
        {
            var response = await Mediator.Send(new GetUnitListQuery());
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost(Router.UnitOfMeasureRouting.Paginated)]
        public async Task<IActionResult> GetUnitPaginatedList([FromBody] GetUnitPaginatedListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost(Router.UnitOfMeasureRouting.Prefix + "getById")]
        public async Task<IActionResult> GetUnitById([FromBody] GetUnitByIdQuery query)
        {
            return NewResult(await Mediator.Send(query));
        }

        [Authorize(Roles = Roles.Admin, Policy = Policies.UnitOfMeasure.Create)]
        [HttpPost(Router.UnitOfMeasureRouting.Create)]
        public async Task<IActionResult> CreateUnit([FromBody] AddUnitCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = Roles.Admin, Policy = Policies.UnitOfMeasure.Edit)]
        [HttpPost(Router.UnitOfMeasureRouting.Edit)]
        public async Task<IActionResult> EditUnit([FromBody] EditUnitCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = Roles.Admin, Policy = Policies.UnitOfMeasure.Delete)]
        [HttpPost(Router.UnitOfMeasureRouting.Prefix + "delete")]
        public async Task<IActionResult> DeleteUnit([FromBody] DeleteUnitCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }
    }
}
