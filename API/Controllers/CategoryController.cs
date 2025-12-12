using Application.Features.Categories.Commands.AddCategory;
using Application.Features.Categories.Commands.EditCategory;
using Application.Features.Categories.Commands.DeleteCategory;
using Application.Features.Categories.Queries.GetCategoryList;
using Application.Features.Categories.Queries.GetCategoryById;
using Application.Features.Categories.Queries.GetCategoryPaginatedList;
using API.Controllers.Base;

namespace API.Controllers
{
    [Authorize]
    public class CategoryController : AppControllerBase
    {
        [AllowAnonymous]
        [HttpGet(Router.CategoryRouting.GetAll)]
        public async Task<IActionResult> GetCategoryList()
        {
            var response = await Mediator.Send(new GetCategoryListQuery());
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet(Router.CategoryRouting.Paginated)]
        public async Task<IActionResult> GetCategoryPaginatedList([FromQuery] GetCategoryPaginatedListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet(Router.CategoryRouting.GetById)]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new GetCategoryByIdQuery(id)));
        }

        [Authorize(Roles = "Admin, Employee")]
        [HttpPost(Router.CategoryRouting.Create)]
        public async Task<IActionResult> CreateCategory([FromBody] AddCategoryCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin, Employee")]
        [HttpPut(Router.CategoryRouting.Edit)]
        public async Task<IActionResult> EditCategory([FromBody] EditCategoryCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete(Router.CategoryRouting.Delete)]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new DeleteCategoryCommand(id)));
        }
    }
}
