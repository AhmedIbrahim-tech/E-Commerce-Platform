using Application.Common.Bases;
using Application.Common.Errors;

namespace Application.Features.Authentication.ResetPassword;

public class ResetPasswordCommandHandler : ApiResponseHandler,
    IRequestHandler<ResetPasswordCommand, ApiResponse<string>>
{
    private readonly IAuthenticationService _authenticationService;

    public ResetPasswordCommandHandler(IAuthenticationService authenticationService) : base()
    {
        _authenticationService = authenticationService;
    }

    public async Task<ApiResponse<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var resetPasswordResult = await _authenticationService.ResetPasswordAsync(request.Email, request.NewPassword);
        return resetPasswordResult switch
        {
            "Success" => Success(""),
            "UserNotFound" => new ApiResponse<string>(UserErrors.UserNotFound()),
            _ => new ApiResponse<string>(AuthenticationErrors.InvalidResetToken())
        };
    }
}

