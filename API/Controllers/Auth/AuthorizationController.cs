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
using Infrastructure.Data.Authorization;

namespace API.Controllers.Auth
{
    [Authorize(Policy = Policies.Admin.ManageUsers)]
    public class AuthorizationController : AppControllerBase
    {
        [Authorize(Policy = Policies.Admin.ManageRoles)]
        [HttpGet(Router.Authorization.GetRoleById)]
        public async Task<IActionResult> GetRoleById([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new GetRoleByIdQuery(id)));
        }

        [Authorize(Policy = Policies.Admin.ManageRoles)]
        [HttpGet(Router.Authorization.GetAllRoles)]
        public async Task<IActionResult> GetRoleList()
        {
            var response = await Mediator.Send(new GetRoleListQuery());
            return NewResult(response);
        }

        [Authorize(Policy = Policies.Admin.ManageRoles)]
        [HttpPost(Router.Authorization.CreateRole)]
        public async Task<IActionResult> CreateRole([FromForm] AddRoleCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Policy = Policies.Admin.ManageRoles)]
        [HttpPut(Router.Authorization.EditRole)]
        public async Task<IActionResult> EditRole([FromForm] EditRoleCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Policy = Policies.Admin.ManageRoles)]
        [HttpDelete(Router.Authorization.DeleteRole)]
        public async Task<IActionResult> DeleteRole([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new DeleteRoleCommand(id)));
        }

        [Authorize(Policy = Policies.Admin.ManageUsers)]
        [SwaggerOperation(OperationId = "ManageUserRoles")]
        [HttpGet(Router.Authorization.ManageUserRoles)]
        public async Task<IActionResult> ManageUserRoles([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new ManageUserRolesQuery(id)));
        }

        [Authorize(Policy = Policies.Admin.ManageUsers)]
        [SwaggerOperation(OperationId = "ManageUserRolesPost")]
        [HttpPost(Router.Authorization.Role + "manageUserRoles")]
        public async Task<IActionResult> ManageUserRolesPost([FromBody] ManageUserRolesQuery query)
        {
            return NewResult(await Mediator.Send(query));
        }

        [Authorize(Policy = Policies.Admin.ManageUsers)]
        [SwaggerOperation(OperationId = "UpdateUserRoles")]
        [HttpPut(Router.Authorization.UpdateUserRoles)]
        public async Task<IActionResult> UpdateUserRoles([FromBody] UpdateUserRolesCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Policy = Policies.Admin.ManageClaims)]
        [SwaggerOperation(OperationId = "ManageUserClaims")]
        [HttpGet(Router.Authorization.ManageUserClaims)]
        public async Task<IActionResult> ManageUserClaims([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new ManageUserClaimsQuery(id)));
        }

        [Authorize(Policy = Policies.Admin.ManageClaims)]
        [SwaggerOperation(OperationId = "ManageUserClaimsPost")]
        [HttpPost(Router.Authorization.Claim + "manageUserClaims")]
        public async Task<IActionResult> ManageUserClaimsPost([FromBody] ManageUserClaimsQuery query)
        {
            return NewResult(await Mediator.Send(query));
        }

        [Authorize(Policy = Policies.Admin.ManageClaims)]
        [SwaggerOperation(OperationId = "UpdateUserClaims")]
        [HttpPut(Router.Authorization.UpdateUserClaims)]
        public async Task<IActionResult> UpdateUserClaims([FromBody] UpdateUserClaimsCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
    }
}
