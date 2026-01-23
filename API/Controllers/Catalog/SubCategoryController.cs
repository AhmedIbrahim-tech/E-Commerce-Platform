using Application.Features.SubCategories.Commands.AddSubCategory;
using Application.Features.SubCategories.Commands.DeleteSubCategory;
using Application.Features.SubCategories.Commands.EditSubCategory;
using Application.Features.SubCategories.Queries.GetSubCategoryById;
using Application.Features.SubCategories.Queries.GetSubCategoryList;
using Application.Features.SubCategories.Queries.GetSubCategoryPaginatedList;
using Infrastructure.Data.Authorization;

namespace API.Controllers.Catalog
{
    [Authorize]
    public class SubCategoryController : AppControllerBase
    {
        [AllowAnonymous]
        [HttpPost(Router.SubCategoryRouting.GetAll)]
        public async Task<IActionResult> GetSubCategoryList([FromBody] GetSubCategoryListQuery? query)
        {
            var response = await Mediator.Send(query ?? new GetSubCategoryListQuery());
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost(Router.SubCategoryRouting.Paginated)]
        public async Task<IActionResult> GetSubCategoryPaginatedList([FromBody] GetSubCategoryPaginatedListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost(Router.SubCategoryRouting.Prefix + "getById")]
        public async Task<IActionResult> GetSubCategoryById([FromBody] GetSubCategoryByIdQuery query)
        {
            return NewResult(await Mediator.Send(query));
        }

        [Authorize(Roles = Roles.Admin, Policy = Policies.SubCategory.Create)]
        [HttpPost(Router.SubCategoryRouting.Create)]
        public async Task<IActionResult> CreateSubCategory([FromBody] AddSubCategoryCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = Roles.Admin, Policy = Policies.SubCategory.Edit)]
        [HttpPost(Router.SubCategoryRouting.Edit)]
        public async Task<IActionResult> EditSubCategory([FromBody] EditSubCategoryCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = Roles.Admin, Policy = Policies.SubCategory.Delete)]
        [HttpPost(Router.SubCategoryRouting.Prefix + "delete")]
        public async Task<IActionResult> DeleteSubCategory([FromBody] DeleteSubCategoryCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }
    }
}
