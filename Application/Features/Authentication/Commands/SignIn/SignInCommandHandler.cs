namespace Application.Features.Authentication.Commands.SignIn;

public class SignInCommandHandler(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    IAuthenticationService authenticationService,
    ApplicationDbContext dbContext,
    IAuditService auditService) : ApiResponseHandler(), 
    IRequestHandler<SignInCommand, ApiResponse<JwtAuthResponse>>
{
    public async Task<ApiResponse<JwtAuthResponse>> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        
        if (user is null)
        {
            _ = auditService.LogEventAsync(
                eventType: "Security",
                eventName: "LoginAttemptFailed",
                description: "Login attempt with non-existent email",
                userEmail: request.Email,
                cancellationToken: cancellationToken
            );
            return new ApiResponse<JwtAuthResponse>(UserErrors.InvalidCredentials());
        }

        if (!user.EmailConfirmed)
        {
            _ = auditService.LogEventAsync(
                eventType: "Security",
                eventName: "LoginAttemptFailed",
                description: "Login attempt with unconfirmed email",
                userId: user.Id,
                userEmail: user.Email,
                cancellationToken: cancellationToken
            );
            return new ApiResponse<JwtAuthResponse>(UserErrors.EmailNotConfirmed());
        }

        var signInResult = await signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
        
        if (signInResult.IsLockedOut)
        {
            _ = auditService.LogEventAsync(
                eventType: "Security",
                eventName: "AccountLocked",
                description: "Login attempt on locked account",
                userId: user.Id,
                userEmail: user.Email,
                cancellationToken: cancellationToken
            );
            return new ApiResponse<JwtAuthResponse>(UserErrors.LockedUser());
        }

        if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow)
        {
            _ = auditService.LogEventAsync(
                eventType: "Security",
                eventName: "AccountLocked",
                description: "Login attempt on locked account (manual lockout)",
                userId: user.Id,
                userEmail: user.Email,
                cancellationToken: cancellationToken
            );
            return new ApiResponse<JwtAuthResponse>(UserErrors.LockedUser());
        }

        if (!signInResult.Succeeded)
        {
            _ = auditService.LogEventAsync(
                eventType: "Security",
                eventName: "LoginAttemptFailed",
                description: "Invalid password provided",
                userId: user.Id,
                userEmail: user.Email,
                cancellationToken: cancellationToken
            );
            return new ApiResponse<JwtAuthResponse>(UserErrors.InvalidCredentials());
        }

        var isDeleted = await dbContext.Customers
            .AnyAsync(c => c.AppUserId == user.Id && c.IsDeleted, cancellationToken) ||
            await dbContext.Vendors
            .AnyAsync(v => v.AppUserId == user.Id && v.IsDeleted, cancellationToken) ||
            await dbContext.Admins
            .AnyAsync(a => a.AppUserId == user.Id && a.IsDeleted, cancellationToken);

        if (isDeleted)
        {
            _ = auditService.LogEventAsync(
                eventType: "Security",
                eventName: "LoginAttemptFailed",
                description: "Login attempt on disabled/deleted account",
                userId: user.Id,
                userEmail: user.Email,
                cancellationToken: cancellationToken
            );
            return new ApiResponse<JwtAuthResponse>(UserErrors.DisabledUser());
        }

        var result = await authenticationService.GetJWTTokenAsync(user);
        
        _ = auditService.LogEventAsync(
            eventType: "Authentication",
            eventName: "UserLogin",
            description: "User logged in successfully",
            userId: user.Id,
            userEmail: user.Email,
            cancellationToken: cancellationToken
        );
        
        return Success(result);
    }
}

