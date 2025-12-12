
using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.Data.Identity;

namespace Application.Features.Authentication.RefreshToken;

public class RefreshTokenCommandHandler : ApiResponseHandler,
    IRequestHandler<RefreshTokenCommand, ApiResponse<JwtAuthResponse>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IAuthenticationService _authenticationService;

    public RefreshTokenCommandHandler(UserManager<AppUser> userManager,
        IAuthenticationService authenticationService) : base()
    {
        _userManager = userManager;
        _authenticationService = authenticationService;
    }

    public async Task<ApiResponse<JwtAuthResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var jwtToken = _authenticationService.ReadJwtToken(request.AccessToken);
        var userIdAndExpireDate = await _authenticationService.ValidateDetails(jwtToken, request.AccessToken, request.RefreshToken);
        switch (userIdAndExpireDate)
        {
            case ("AlgorithmIsWrong", null): return new ApiResponse<JwtAuthResponse>(UserErrors.InvalidJwtToken());
            case ("TokenIsNotExpired", null): return new ApiResponse<JwtAuthResponse>(UserErrors.InvalidJwtToken());
            case ("RefreshTokenIsNotFound", null): return new ApiResponse<JwtAuthResponse>(UserErrors.InvalidRefreshToken());
            case ("RefreshTokenIsExpired", null): return new ApiResponse<JwtAuthResponse>(UserErrors.InvalidRefreshToken());
        }
        var (userId, expiryDate) = userIdAndExpireDate;
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new ApiResponse<JwtAuthResponse>(UserErrors.UserNotFound());
        }
        var result = await _authenticationService.GetRefreshTokenAsync(user, jwtToken, expiryDate, request.RefreshToken);
        return Success(result);
    }
}

