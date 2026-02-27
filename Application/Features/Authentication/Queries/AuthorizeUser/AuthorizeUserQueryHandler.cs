
using Application.Common.Bases;
using Application.Common.Errors;

namespace Application.Features.Authentication.AuthorizeUser;

public class AuthorizeUserQueryHandler : ApiResponseHandler,
    IRequestHandler<AuthorizeUserQuery, ApiResponse<string>>
{
    private readonly IAuthenticationService _authenticationService;

    public AuthorizeUserQueryHandler(IAuthenticationService authenticationService) : base()
    {
        _authenticationService = authenticationService;
    }

    public async Task<ApiResponse<string>> Handle(AuthorizeUserQuery request, CancellationToken cancellationToken)
    {
        var (isValid, message) = await _authenticationService.ValidateToken(request.AccessToken);
        if (isValid)
            return Success(message);
        return new ApiResponse<string>(UserErrors.InvalidJwtToken());
    }
}

