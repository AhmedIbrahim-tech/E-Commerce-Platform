using Application.ServicesHandlers.Auth;

namespace Application.Features.Authentication.Commands.Logout;

public class LogoutCommandHandler(
    IAuthenticationService authenticationService,
    ICurrentUserService currentUserService) : ApiResponseHandler(),
    IRequestHandler<LogoutCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        if (!currentUserService.IsAuthenticated)
        {
            return new ApiResponse<string>(UserErrors.InvalidCredentials());
        }

        var userId = currentUserService.GetUserId();
        var result = await authenticationService.LogoutAsync(userId, request.RefreshToken);

        if (result)
        {
            return Success("Logged out successfully");
        }

        return new ApiResponse<string>(UserErrors.InvalidRefreshToken());
    }
}
