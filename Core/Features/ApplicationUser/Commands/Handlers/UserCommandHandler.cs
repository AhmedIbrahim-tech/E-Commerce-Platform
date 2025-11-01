using Core.Features.ApplicationUser.Commands.Models;

namespace Core.Features.ApplicationUser.Commands.Handlers
{
    internal class UserCommandHandler(UserManager<User> userManager, IMapper mapper, IApplicationUserService applicationUserService) : ApiResponseHandler(),
        IRequestHandler<AddCustomerCommand, ApiResponse<string>>,
        IRequestHandler<ChangeUserPasswordCommand, ApiResponse<string>>
    {

        #region Constructors
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<string>> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.Id.ToString());
            if (user is null) return NotFound<string>();

            var changePasswordResult = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!changePasswordResult.Succeeded)
                return BadRequest<string>(changePasswordResult.Errors.FirstOrDefault()?.Description);
            return Success<string>(SharedResourcesKeys.PasswordChangedSuccessfully);
        }

        public async Task<ApiResponse<string>> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
        {
            var identityUser = mapper.Map<Customer>(request);
            var createResult = await applicationUserService.AddUserAsync(identityUser, request.Password);
            return createResult switch
            {
                "EmailIsExist" => BadRequest<string>(SharedResourcesKeys.EmailIsExist),
                "UserNameIsExist" => BadRequest<string>(SharedResourcesKeys.UserNameIsExist),
                "FailedToAddNewRoles" => BadRequest<string>(SharedResourcesKeys.FailedToAddNewRoles),
                "FailedToAddNewClaims" => BadRequest<string>(SharedResourcesKeys.FailedToAddNewClaims),
                "SendEmailFailed" => BadRequest<string>(SharedResourcesKeys.SendEmailFailed),
                "Failed" => BadRequest<string>(SharedResourcesKeys.TryToRegisterAgain),
                "Success" => Created(""),
                _ => BadRequest<string>(createResult)
            };

            #region Old Style of Switch Case
            //switch (createResult)
            //{
            //    case "EmailIsExist": return BadRequest<string>(SharedResourcesKeys.EmailIsExist);
            //    case "UserNameIsExist": return BadRequest<string>(SharedResourcesKeys.UserNameIsExist);
            //    case "FailedToAddNewRoles": return BadRequest<string>(SharedResourcesKeys.FailedToAddNewRoles);
            //    case "FailedToAddNewClaims": return BadRequest<string>(SharedResourcesKeys.FailedToAddNewClaims);
            //    case "SendEmailFailed": return BadRequest<string>(SharedResourcesKeys.SendEmailFailed);
            //    case "Failed": return BadRequest<string>(SharedResourcesKeys.TryToRegisterAgain);
            //    case "Success": return Created("");
            //    default: return BadRequest<string>(createResult);
            //} 
            #endregion
        }
        #endregion
    }
}
