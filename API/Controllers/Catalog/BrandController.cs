using Application.Features.Brands.Commands.AddBrand;
using Application.Features.Brands.Commands.DeleteBrand;
using Application.Features.Brands.Commands.EditBrand;
using Application.Features.Brands.Queries.GetBrandById;
using Application.Features.Brands.Queries.GetBrandList;
using Application.Features.Brands.Queries.GetBrandPaginatedList;
using Infrastructure.Data.Authorization;

namespace API.Controllers.Catalog
{
    [Authorize]
    public class BrandController : AppControllerBase
    {
        [AllowAnonymous]
        [HttpPost(Router.BrandRouting.GetAll)]
        public async Task<IActionResult> GetBrandList()
        {
            var response = await Mediator.Send(new GetBrandListQuery());
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost(Router.BrandRouting.Paginated)]
        public async Task<IActionResult> GetBrandPaginatedList([FromBody] GetBrandPaginatedListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost(Router.BrandRouting.Prefix + "getById")]
        public async Task<IActionResult> GetBrandById([FromBody] GetBrandByIdQuery query)
        {
            return NewResult(await Mediator.Send(query));
        }

        [Authorize(Roles = Roles.Admin, Policy = Policies.Brand.Create)]
        [HttpPost(Router.BrandRouting.Create)]
        public async Task<IActionResult> CreateBrand([FromBody] AddBrandCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = Roles.Admin, Policy = Policies.Brand.Edit)]
        [HttpPost(Router.BrandRouting.Edit)]
        public async Task<IActionResult> EditBrand([FromBody] EditBrandCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = Roles.Admin, Policy = Policies.Brand.Delete)]
        [HttpPost(Router.BrandRouting.Prefix + "delete")]
        public async Task<IActionResult> DeleteBrand([FromBody] DeleteBrandCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }
    }
}
