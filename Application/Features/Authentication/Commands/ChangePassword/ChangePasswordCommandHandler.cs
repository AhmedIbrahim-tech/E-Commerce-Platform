using Application.ServicesHandlers.Auth;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Authentication.Commands.ChangePassword;

public class ChangePasswordCommandHandler(
    UserManager<AppUser> userManager,
    ICurrentUserService currentUserService) : ApiResponseHandler(),
    IRequestHandler<ChangePasswordCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        if (!currentUserService.IsAuthenticated)
            return new ApiResponse<string>(UserErrors.InvalidCredentials());

        var user = await userManager.FindByIdAsync(currentUserService.GetUserId().ToString());
        if (user is null)
            return new ApiResponse<string>(UserErrors.UserNotFound());

        var changePasswordResult = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!changePasswordResult.Succeeded)
            return new ApiResponse<string>(UserErrors.InvalidCredentials());

        return Success<string>("Password changed successfully");
    }
}
