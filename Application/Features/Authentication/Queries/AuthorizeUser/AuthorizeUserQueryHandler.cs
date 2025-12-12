
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
        var result = await _authenticationService.ValidateToken(request.AccessToken);
        if (result == "Not Expired.")
            return Success(result);
        return new ApiResponse<string>(UserErrors.InvalidJwtToken());
    }
}

