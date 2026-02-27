using Application.ServicesHandlers.Services;

namespace Application.Features.Authentication.Commands.ResetPassword;

public class ResetPasswordCommandHandler(IAuthenticationService authenticationService) : ApiResponseHandler(),
    IRequestHandler<ResetPasswordCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var resetPasswordResult = await authenticationService.ResetPasswordAsync(request.Email, request.NewPassword);
        return resetPasswordResult switch
        {
            AuthenticationService.ResultSuccess => Success(""),
            AuthenticationService.ResultUserNotFound => new ApiResponse<string>(UserErrors.UserNotFound()),
            _ => new ApiResponse<string>(AuthenticationErrors.InvalidResetToken())
        };
    }
}

