using Application.Common.Constants;
using Application.Common.Errors;

namespace Application.Features.Authentication.ConfirmResetPassword;

public class ConfirmResetPasswordQueryHandler(IAuthenticationService authenticationService) : ApiResponseHandler(),
    IRequestHandler<ConfirmResetPasswordQuery, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(ConfirmResetPasswordQuery request, CancellationToken cancellationToken)
    {
        var result = await authenticationService.ConfirmResetPasswordAsync(request.Code, request.Email);
        return result switch
        {
            AuthenticationResult.UserNotFound => new ApiResponse<string>(UserErrors.UserNotFound()),
            AuthenticationResult.Success => Success(""),
            _ => new ApiResponse<string>(AuthenticationErrors.InvalidVerificationCode())
        };
    }
}

