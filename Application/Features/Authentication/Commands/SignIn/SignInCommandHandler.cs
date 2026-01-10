using Application.Common.Helpers;

namespace Application.Features.Authentication.Commands.SignIn;

public class SignInCommandHandler(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    IAuthenticationService authenticationService) : ApiResponseHandler(), 
    IRequestHandler<SignInCommand, ApiResponse<JwtAuthResponse>>
{
    public async Task<ApiResponse<JwtAuthResponse>> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.UserName);
        if (user is null) return new ApiResponse<JwtAuthResponse>(UserErrors.InvalidCredentials());

        var signInResult = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!user.EmailConfirmed) return new ApiResponse<JwtAuthResponse>(UserErrors.EmailNotConfirmed());
        
        if (signInResult.IsLockedOut) return new ApiResponse<JwtAuthResponse>(UserErrors.LockedUser());
        if (!user.LockoutEnabled && user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow)
            return new ApiResponse<JwtAuthResponse>(UserErrors.LockedUser());
        
        if (!signInResult.Succeeded) return new ApiResponse<JwtAuthResponse>(UserErrors.InvalidCredentials());

        var result = await authenticationService.GetJWTTokenAsync(user);
        return Success(result);
    }
}

