using Application.Common.Constants;
using Application.Common.Helpers;

namespace Application.Features.Authentication.Commands.GoogleLogin;

public class GoogleLoginCommandHandler(IAuthGoogleService authGoogleService) : ApiResponseHandler(),
    IRequestHandler<GoogleLoginCommand, ApiResponse<JwtAuthResponse>>
{
    public async Task<ApiResponse<JwtAuthResponse>> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
    {
        var (response, message) = await authGoogleService.AuthenticateWithGoogleAsync(request.IdToken);
        return message switch
        {
            AuthenticationResult.Success => Success(response),
            "InvalidGoogleToken" => new ApiResponse<JwtAuthResponse>(AuthenticationErrors.SocialLoginFailed()),
            "FailedToAddNewRoles" => new ApiResponse<JwtAuthResponse>(RoleErrors.InvalidPermissions()),
            "FailedToAddNewClaims" => new ApiResponse<JwtAuthResponse>(PermissionErrors.PermissionNotAssigned()),
            "GoogleAuthenticationFailed" => new ApiResponse<JwtAuthResponse>(AuthenticationErrors.SocialLoginFailed()),
            _ => new ApiResponse<JwtAuthResponse>(AuthenticationErrors.SocialLoginFailed())
        };
    }
}

