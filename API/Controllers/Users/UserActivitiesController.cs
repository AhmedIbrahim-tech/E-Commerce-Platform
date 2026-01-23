using Application.Features.ApplicationUser.Queries.GetMyActivitiesPaginatedList;
using API.Controllers.Base;
using Infrastructure.Data.Authorization;

namespace API.Controllers.Users;

[Authorize]
public class UserActivitiesController : AppControllerBase
{
    [Authorize(Policy = Policies.Auth.ViewOwnProfile)]
    [HttpPost(Router.UserRouting.MyActivitiesPaginated)]
    public async Task<IActionResult> GetMyActivitiesPaginatedList([FromBody] GetMyActivitiesPaginatedListQuery query)
    {
        var response = await Mediator.Send(query);
        return Ok(response);
    }
}

