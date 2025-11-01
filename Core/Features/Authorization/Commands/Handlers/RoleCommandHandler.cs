using Core.Features.Authorization.Commands.Models;

namespace Core.Features.Authorization.Commands.Handlers
{
    public class RoleCommandHandler : ApiResponseHandler,
        IRequestHandler<AddRoleCommand, ApiResponse<string>>,
        IRequestHandler<EditRoleCommand, ApiResponse<string>>,
        IRequestHandler<DeleteRoleCommand, ApiResponse<string>>,
        IRequestHandler<UpdateUserRolesCommand, ApiResponse<string>>
    {
        #region Fields
        private readonly IAuthorizationService _authorizationService;
        #endregion

        #region Constructors
        public RoleCommandHandler(IAuthorizationService authorizationService) : base()
        {
            _authorizationService = authorizationService;        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<string>> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorizationService.AddRoleAsync(request.RoleName);
            if (result == "Success") return Created("");
            return BadRequest<string>(SharedResourcesKeys.CreateFailed);
        }

        public async Task<ApiResponse<string>> Handle(EditRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorizationService.EditRoleAsync(request.RoleName, request.RoleId);
            if (result == "NotFound") return NotFound<string>();
            else if (result == "Success") return Edit("");
            return BadRequest<string>(SharedResourcesKeys.UpdateFailed);
        }

        public async Task<ApiResponse<string>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorizationService.DeleteRoleAsync(request.RoleId);
            if (result == "NotFound") return NotFound<string>();
            else if (result == "Success") return Deleted<string>();
            else if (result == "Used") return BadRequest<string>(SharedResourcesKeys.RoleIsUsed);
            return BadRequest<string>(result);
        }

        public async Task<ApiResponse<string>> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorizationService.UpdateUserRoles(request);
            switch (result)
            {
                case "UserIsNull": return NotFound<string>();
                case "FailedToRemoveOldRoles": return BadRequest<string>(SharedResourcesKeys.FailedToRemoveOldRoles);
                case "FailedToAddNewRoles": return BadRequest<string>(SharedResourcesKeys.FailedToAddNewRoles);
                case "FailedToUpdateUserRoles": return BadRequest<string>(SharedResourcesKeys.FailedToUpdateUserRoles);
            }
            return Edit("");
        }
        #endregion
    }
}
