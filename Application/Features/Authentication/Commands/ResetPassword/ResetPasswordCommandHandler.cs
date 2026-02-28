using Application.Common.Constants;
using Application.ServicesHandlers.Services;

namespace Application.Features.Authentication.Commands.ResetPassword;

public class ResetPasswordCommandHandler(IAuthenticationService authenticationService) : ApiResponseHandler(),
    IRequestHandler<ResetPasswordCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var result = await authenticationService.ResetPasswordAsync(request.Email, request.NewPassword);
        return result switch
        {
            AuthenticationResult.Success => Success(""),
            AuthenticationResult.UserNotFound => new ApiResponse<string>(UserErrors.UserNotFound()),
            _ => new ApiResponse<string>(AuthenticationErrors.InvalidResetToken())
        };
    }
}

