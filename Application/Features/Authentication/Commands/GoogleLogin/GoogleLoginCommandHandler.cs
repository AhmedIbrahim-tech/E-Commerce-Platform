using Application.Common.Bases;
using Application.Common.Errors;

namespace Application.Features.Authentication.GoogleLogin;

public class GoogleLoginCommandHandler : ApiResponseHandler,
    IRequestHandler<GoogleLoginCommand, ApiResponse<JwtAuthResponse>>
{
    private readonly IAuthGoogleService _authGoogleService;

    public GoogleLoginCommandHandler(IAuthGoogleService authGoogleService) : base()
    {
        _authGoogleService = authGoogleService;
    }

    public async Task<ApiResponse<JwtAuthResponse>> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
    {
        var (response, message) = await _authGoogleService.AuthenticateWithGoogleAsync(request.IdToken);
        return message switch
        {
            "Success" => Success(response),
            "InvalidGoogleToken" => new ApiResponse<JwtAuthResponse>(AuthenticationErrors.SocialLoginFailed()),
            "FailedToAddNewRoles" => new ApiResponse<JwtAuthResponse>(RoleErrors.InvalidPermissions()),
            "FailedToAddNewClaims" => new ApiResponse<JwtAuthResponse>(PermissionErrors.PermissionNotAssigned()),
            "GoogleAuthenticationFailed" => new ApiResponse<JwtAuthResponse>(AuthenticationErrors.SocialLoginFailed()),
            _ => new ApiResponse<JwtAuthResponse>(AuthenticationErrors.SocialLoginFailed()),
        };
    }
}

