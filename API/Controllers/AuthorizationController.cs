using Application.Features.Authorization.Commands.AddRole;
using Application.Features.Authorization.Commands.EditRole;
using Application.Features.Authorization.Commands.DeleteRole;
using Application.Features.Authorization.Commands.UpdateUserRoles;
using Application.Features.Authorization.Commands.UpdateUserClaims;
using Application.Features.Authorization.Queries.GetRoleById;
using Application.Features.Authorization.Queries.GetRoleList;
using Application.Features.Authorization.Queries.ManageUserRoles;
using Application.Features.Authorization.Queries.ManageUserClaims;
using API.Controllers.Base;

namespace API.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuthorizationController : AppControllerBase
    {
        [HttpGet(Router.Authorization.GetRoleById)]
        public async Task<IActionResult> GetRoleById([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new GetRoleByIdQuery(id)));
        }

        [HttpGet(Router.Authorization.GetAllRoles)]
        public async Task<IActionResult> GetRoleList()
        {
            var response = await Mediator.Send(new GetRoleListQuery());
            return Ok(response);
        }

        [HttpPost(Router.Authorization.CreateRole)]
        public async Task<IActionResult> CreateRole([FromForm] AddRoleCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut(Router.Authorization.EditRole)]
        public async Task<IActionResult> EditRole([FromForm] EditRoleCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete(Router.Authorization.DeleteRole)]
        public async Task<IActionResult> DeleteRole([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new DeleteRoleCommand(id)));
        }

        [SwaggerOperation(OperationId = "ManageUserRoles")]
        [HttpGet(Router.Authorization.ManageUserRoles)]
        public async Task<IActionResult> ManageUserRoles([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new ManageUserRolesQuery(id)));
        }

        [SwaggerOperation(OperationId = "UpdateUserRoles")]
        [HttpPut(Router.Authorization.UpdateUserRoles)]
        public async Task<IActionResult> UpdateUserRoles([FromBody] UpdateUserRolesCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
        [SwaggerOperation(OperationId = "ManageUserClaims")]
        [HttpGet(Router.Authorization.ManageUserClaims)]
        public async Task<IActionResult> ManageUserClaims([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new ManageUserClaimsQuery(id)));
        }

        [SwaggerOperation(OperationId = "UpdateUserClaims")]
        [HttpPut(Router.Authorization.UpdateUserClaims)]
        public async Task<IActionResult> UpdateUserClaims([FromBody] UpdateUserClaimsCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
    }
}
