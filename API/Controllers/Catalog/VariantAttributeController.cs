using Application.Features.VariantAttributes.Commands.AddVariantAttribute;
using Application.Features.VariantAttributes.Commands.DeleteVariantAttribute;
using Application.Features.VariantAttributes.Commands.EditVariantAttribute;
using Application.Features.VariantAttributes.Queries.GetVariantAttributeById;
using Application.Features.VariantAttributes.Queries.GetVariantAttributeList;
using Application.Features.VariantAttributes.Queries.GetVariantAttributePaginatedList;
using Infrastructure.Data.Authorization;

namespace API.Controllers.Catalog
{
    [Authorize]
    public class VariantAttributeController : AppControllerBase
    {
        [AllowAnonymous]
        [HttpPost(Router.VariantAttributeRouting.GetAll)]
        public async Task<IActionResult> GetVariantAttributeList()
        {
            var response = await Mediator.Send(new GetVariantAttributeListQuery());
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost(Router.VariantAttributeRouting.Paginated)]
        public async Task<IActionResult> GetVariantAttributePaginatedList([FromBody] GetVariantAttributePaginatedListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost(Router.VariantAttributeRouting.Prefix + "getById")]
        public async Task<IActionResult> GetVariantAttributeById([FromBody] GetVariantAttributeByIdQuery query)
        {
            return NewResult(await Mediator.Send(query));
        }

        [Authorize(Roles = Roles.Admin, Policy = Policies.VariantAttribute.Create)]
        [HttpPost(Router.VariantAttributeRouting.Create)]
        public async Task<IActionResult> CreateVariantAttribute([FromBody] AddVariantAttributeCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = Roles.Admin, Policy = Policies.VariantAttribute.Edit)]
        [HttpPost(Router.VariantAttributeRouting.Edit)]
        public async Task<IActionResult> EditVariantAttribute([FromBody] EditVariantAttributeCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = Roles.Admin, Policy = Policies.VariantAttribute.Delete)]
        [HttpPost(Router.VariantAttributeRouting.Prefix + "delete")]
        public async Task<IActionResult> DeleteVariantAttribute([FromBody] DeleteVariantAttributeCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }
    }
}
