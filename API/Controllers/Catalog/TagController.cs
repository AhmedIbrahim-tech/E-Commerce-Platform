using Application.Features.Tags.Commands.AddTag;
using Application.Features.Tags.Commands.DeleteTag;
using Application.Features.Tags.Commands.EditTag;
using Application.Features.Tags.Queries.GetTagById;
using Application.Features.Tags.Queries.GetTagList;
using Application.Features.Tags.Queries.GetTagPaginatedList;
using Infrastructure.Data.Authorization;

namespace API.Controllers.Catalog;

[Authorize]
public class TagController : AppControllerBase
{
    [AllowAnonymous]
    [HttpPost(Router.TagRouting.GetAll)]
    public async Task<IActionResult> GetTagList()
    {
        var response = await Mediator.Send(new GetTagListQuery());
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost(Router.TagRouting.Paginated)]
    public async Task<IActionResult> GetTagPaginatedList([FromBody] GetTagPaginatedListQuery query)
    {
        var response = await Mediator.Send(query);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost(Router.TagRouting.GetById)]
    public async Task<IActionResult> GetTagById([FromBody] GetTagByIdQuery query)
    {
        return NewResult(await Mediator.Send(query));
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPost(Router.TagRouting.Create)]
    public async Task<IActionResult> CreateTag([FromBody] AddTagCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPost(Router.TagRouting.Edit)]
    public async Task<IActionResult> EditTag([FromBody] EditTagCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPost(Router.TagRouting.Delete)]
    public async Task<IActionResult> DeleteTag([FromBody] DeleteTagCommand command)
    {
        return NewResult(await Mediator.Send(command));
    }
}

