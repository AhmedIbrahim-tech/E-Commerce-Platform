using Application.ServicesHandlers.Auth;

namespace Application.Features.Authentication.Commands.LogoutAll;

public class LogoutAllCommandHandler(
    IAuthenticationService authenticationService,
    ICurrentUserService currentUserService) : ApiResponseHandler(),
    IRequestHandler<LogoutAllCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(LogoutAllCommand request, CancellationToken cancellationToken)
    {
        if (!currentUserService.IsAuthenticated)
        {
            return new ApiResponse<string>(UserErrors.InvalidCredentials());
        }

        var userId = currentUserService.GetUserId();
        var result = await authenticationService.LogoutAllSessionsAsync(userId);

        if (result)
        {
            return Success("Logged out from all sessions successfully");
        }

        return Success("No active sessions found");
    }
}
