using Application.Common.Bases;
using Application.Common.Errors;

namespace Application.Features.Authentication.ConfirmResetPassword;

public class ConfirmResetPasswordQueryHandler : ApiResponseHandler,
    IRequestHandler<ConfirmResetPasswordQuery, ApiResponse<string>>
{
    private readonly IAuthenticationService _authenticationService;

    public ConfirmResetPasswordQueryHandler(IAuthenticationService authenticationService) : base()
    {
        _authenticationService = authenticationService;
    }

    public async Task<ApiResponse<string>> Handle(ConfirmResetPasswordQuery request, CancellationToken cancellationToken)
    {
        var confirmResetPasswordResult = await _authenticationService.ConfirmResetPasswordAsync(request.Code, request.Email);
        return confirmResetPasswordResult switch
        {
            "UserNotFound" => new ApiResponse<string>(UserErrors.UserNotFound()),
            "Success" => Success(""),
            _ => new ApiResponse<string>(AuthenticationErrors.InvalidVerificationCode())
        };
    }
}

