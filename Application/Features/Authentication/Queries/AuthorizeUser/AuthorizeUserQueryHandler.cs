using Application.Common.Bases;
using Application.Common.Errors;

namespace Application.Features.Authentication.AuthorizeUser;

public class AuthorizeUserQueryHandler(IAuthenticationService authenticationService) : ApiResponseHandler(),
    IRequestHandler<AuthorizeUserQuery, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(AuthorizeUserQuery request, CancellationToken cancellationToken)
    {
        var (isValid, message) = await authenticationService.ValidateToken(request.AccessToken);
        return isValid ? Success(message) : new ApiResponse<string>(UserErrors.InvalidJwtToken());
    }
}

