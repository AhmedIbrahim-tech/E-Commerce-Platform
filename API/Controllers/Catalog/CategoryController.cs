using Application.Features.Categories.Commands.AddCategory;
using Application.Features.Categories.Commands.DeleteCategory;
using Application.Features.Categories.Commands.EditCategory;
using Application.Features.Categories.Queries.GetCategoryById;
using Application.Features.Categories.Queries.GetCategoryList;
using Application.Features.Categories.Queries.GetCategoryPaginatedList;
using Infrastructure.Data.Authorization;

namespace API.Controllers.Catalog;

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
    [HttpPost(Router.CategoryRouting.Paginated)]
    public async Task<IActionResult> GetCategoryPaginatedList([FromBody] GetCategoryPaginatedListQuery query)
    {
        var response = await Mediator.Send(query);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost(Router.CategoryRouting.GetById)]
    public async Task<IActionResult> GetCategoryById([FromBody] GetCategoryByIdQuery query)
    {
        return NewResult(await Mediator.Send(query));
    }

    [Authorize(Roles = Roles.Admin, Policy = Policies.Category.Create)]
    [HttpPost(Router.CategoryRouting.Create)]
    public async Task<IActionResult> CreateCategory([FromBody] AddCategoryCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    [Authorize(Roles = Roles.Admin, Policy = Policies.Category.Edit)]
    [HttpPut(Router.CategoryRouting.Edit)]
    public async Task<IActionResult> EditCategory([FromBody] EditCategoryCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    [Authorize(Roles = Roles.Admin, Policy = Policies.Category.Delete)]
    [HttpPost(Router.CategoryRouting.Delete)]
    public async Task<IActionResult> DeleteCategory([FromBody] DeleteCategoryCommand command)
    {
        return NewResult(await Mediator.Send(command));
    }
}
