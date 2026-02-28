using Application.Common.Constants;
using Application.ServicesHandlers.Services;

namespace Application.Features.Authentication.ConfirmEmail;

public class ConfirmEmailQueryHandler(IAuthenticationService authenticationService) : ApiResponseHandler(),
    IRequestHandler<ConfirmEmailQuery, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(ConfirmEmailQuery request, CancellationToken cancellationToken)
    {
        var result = await authenticationService.ConfirmEmailAsync(request.UserId, request.Code);
        return result switch
        {
            AuthenticationResult.UserOrCodeIsNullOrEmpty => new ApiResponse<string>(UserErrors.InvalidCode()),
            AuthenticationResult.Success => Success<string>("ConfirmEmailDone"),
            _ => new ApiResponse<string>(UserErrors.InvalidCode())
        };
    }
}

