using Core.Features.Authorization.Commands.Models;

namespace Core.Features.Authorization.Commands.Handlers
{
    public class ClaimsCommandHandler : ApiResponseHandler,
        IRequestHandler<UpdateUserClaimsCommand, ApiResponse<string>>
    {
        #region Fields
        private readonly IAuthorizationService _authorizationService;
        #endregion

        #region Constructors
        public ClaimsCommandHandler(IAuthorizationService authorizationService) : base()
        {
            _authorizationService = authorizationService;        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<string>> Handle(UpdateUserClaimsCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorizationService.UpdateUserClaims(request);
            switch (result)
            {
                case "UserIsNull": return NotFound<string>();
                case "FailedToRemoveOldClaims": return BadRequest<string>(SharedResourcesKeys.FailedToRemoveOldClaims);
                case "FailedToAddNewClaims": return BadRequest<string>(SharedResourcesKeys.FailedToAddNewClaims);
                case "FailedToUpdateUserClaims": return BadRequest<string>(SharedResourcesKeys.FailedToUpdateUserClaims);
            }
            return Edit("");
        }
        #endregion
    }
}
