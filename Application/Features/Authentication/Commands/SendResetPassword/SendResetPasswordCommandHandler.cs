using Application.ServicesHandlers.Services;

namespace Application.Features.Authentication.Commands.SendResetPassword;

public class SendResetPasswordCommandHandler : ApiResponseHandler,
    IRequestHandler<SendResetPasswordCommand, ApiResponse<string>>
{
    private readonly IAuthenticationService _authenticationService;

    public SendResetPasswordCommandHandler(IAuthenticationService authenticationService) : base()
    {
        _authenticationService = authenticationService;
    }

    public async Task<ApiResponse<string>> Handle(SendResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var resetPasswordResult = await _authenticationService.SendResetPasswordCodeAsync(request.Email);
        return resetPasswordResult switch
        {
            AuthenticationService.ResultSuccess => Success(""),
            AuthenticationService.ResultUserNotFound => new ApiResponse<string>(UserErrors.UserNotFound()),
            _ => new ApiResponse<string>(EmailErrors.EmailSendFailed())
        };
    }
}

