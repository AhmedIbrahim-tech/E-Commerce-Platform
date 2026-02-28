using Application.Common.Constants;
using Application.ServicesHandlers.Services;

namespace Application.Features.Authentication.Commands.SendResetPassword;

public class SendResetPasswordCommandHandler(IAuthenticationService authenticationService) : ApiResponseHandler(),
    IRequestHandler<SendResetPasswordCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(SendResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var result = await authenticationService.SendResetPasswordCodeAsync(request.Email);
        return result switch
        {
            AuthenticationResult.Success => Success(""),
            AuthenticationResult.UserNotFound => new ApiResponse<string>(UserErrors.UserNotFound()),
            _ => new ApiResponse<string>(EmailErrors.EmailSendFailed())
        };
    }
}

