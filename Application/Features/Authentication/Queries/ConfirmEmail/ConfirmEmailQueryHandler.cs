using Application.ServicesHandlers.Services;

namespace Application.Features.Authentication.ConfirmEmail;

public class ConfirmEmailQueryHandler : ApiResponseHandler,
    IRequestHandler<ConfirmEmailQuery, ApiResponse<string>>
{
    private readonly IAuthenticationService _authenticationService;

    public ConfirmEmailQueryHandler(IAuthenticationService authenticationService) : base()
    {
        _authenticationService = authenticationService;
    }

    public async Task<ApiResponse<string>> Handle(ConfirmEmailQuery request, CancellationToken cancellationToken)
    {
        var confirmEmailResult = await _authenticationService.ConfirmEmailAsync(request.UserId, request.Code);
        return confirmEmailResult switch
        {
            AuthenticationService.ResultUserOrCodeIsNullOrEmpty => new ApiResponse<string>(UserErrors.InvalidCode()),
            AuthenticationService.ResultSuccess => Success<string>("ConfirmEmailDone"),
            _ => new ApiResponse<string>(UserErrors.InvalidCode())
        };
    }
}

