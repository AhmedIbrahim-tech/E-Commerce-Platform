using Application.Common.Helpers;

namespace Application.Features.Authentication.Commands.RefreshToken;

public class RefreshTokenCommandHandler(UserManager<AppUser> userManager,
    IAuthenticationService authenticationService) : ApiResponseHandler(),
    IRequestHandler<RefreshTokenCommand, ApiResponse<JwtAuthResponse>>
{
    public async Task<ApiResponse<JwtAuthResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var jwtToken = authenticationService.ReadJwtToken(request.AccessToken);
        var userIdAndExpireDate = await authenticationService.ValidateDetails(jwtToken, request.AccessToken, request.RefreshToken);
        switch (userIdAndExpireDate)
        {
            case ("AlgorithmIsWrong", null): return new ApiResponse<JwtAuthResponse>(UserErrors.InvalidJwtToken());
            case ("TokenIsNotExpired", null): return new ApiResponse<JwtAuthResponse>(UserErrors.InvalidJwtToken());
            case ("RefreshTokenIsNotFound", null): return new ApiResponse<JwtAuthResponse>(UserErrors.InvalidRefreshToken());
            case ("RefreshTokenIsExpired", null): return new ApiResponse<JwtAuthResponse>(UserErrors.InvalidRefreshToken());
        }
        var (userId, expiryDate) = userIdAndExpireDate;
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new ApiResponse<JwtAuthResponse>(UserErrors.UserNotFound());
        }
        var result = await authenticationService.GetRefreshTokenAsync(user, jwtToken, expiryDate, request.RefreshToken);
        return Success(result);
    }
}

