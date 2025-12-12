using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.Data.Identity;

namespace Application.Features.ApplicationUser.Commands.ChangeUserPassword
{
    internal class ChangeUserPasswordCommandHandler(UserManager<AppUser> userManager) : ApiResponseHandler(),
        IRequestHandler<ChangeUserPasswordCommand, ApiResponse<string>>
    {
        public async Task<ApiResponse<string>> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.Id.ToString());
            if (user is null) return new ApiResponse<string>(UserErrors.UserNotFound());

            var changePasswordResult = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!changePasswordResult.Succeeded)
                return new ApiResponse<string>(UserErrors.InvalidCredentials());
            return Success<string>("Password changed successfully");
        }
    }
}

