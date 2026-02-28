using Application.Common.Bases;
using Application.Common.Constants;
using Application.Common.Errors;
using Application.Common.Helpers;
using Application.ServicesHandlers.Auth;
using Application.ServicesHandlers.Services;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Authentication.Commands.SignIn;

public class SignInCommandHandler(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    IAuthenticationService authenticationService,
    IUserStatusService userStatusService,
    IAuditService auditService) : ApiResponseHandler(),
    IRequestHandler<SignInCommand, ApiResponse<JwtAuthResponse>>
{
    public async Task<ApiResponse<JwtAuthResponse>> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            _ = auditService.LogEventAsync(AuditEventType.Security, AuditEventName.LoginAttemptFailed, "Login attempt with non-existent email", userEmail: request.Email, cancellationToken: cancellationToken);
            return new ApiResponse<JwtAuthResponse>(UserErrors.InvalidCredentials());
        }

        if (!user.EmailConfirmed)
        {
            _ = auditService.LogEventAsync(AuditEventType.Security, AuditEventName.LoginAttemptFailed, "Login attempt with unconfirmed email", user.Id, user.Email, cancellationToken: cancellationToken);
            return new ApiResponse<JwtAuthResponse>(UserErrors.EmailNotConfirmed());
        }

        var signInResult = await signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);

        if (signInResult.IsLockedOut)
        {
            _ = auditService.LogEventAsync(AuditEventType.Security, AuditEventName.AccountLocked, "Login attempt on locked account", user.Id, user.Email, cancellationToken: cancellationToken);
            return new ApiResponse<JwtAuthResponse>(UserErrors.LockedUser());
        }

        if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow)
        {
            _ = auditService.LogEventAsync(AuditEventType.Security, AuditEventName.AccountLocked, "Login attempt on locked account (manual lockout)", user.Id, user.Email, cancellationToken: cancellationToken);
            return new ApiResponse<JwtAuthResponse>(UserErrors.LockedUser());
        }

        if (!signInResult.Succeeded)
        {
            _ = auditService.LogEventAsync(AuditEventType.Security, AuditEventName.LoginAttemptFailed, "Invalid password provided", user.Id, user.Email, cancellationToken: cancellationToken);
            return new ApiResponse<JwtAuthResponse>(UserErrors.InvalidCredentials());
        }

        if (await userStatusService.IsUserDeletedAsync(user.Id, cancellationToken))
        {
            _ = auditService.LogEventAsync(AuditEventType.Security, AuditEventName.LoginAttemptFailed, "Login attempt on disabled/deleted account", user.Id, user.Email, cancellationToken: cancellationToken);
            return new ApiResponse<JwtAuthResponse>(UserErrors.DisabledUser());
        }

        var result = await authenticationService.GetJWTTokenAsync(user, request.RememberMe);
        _ = auditService.LogEventAsync(AuditEventType.Authentication, AuditEventName.UserLogin, "User logged in successfully", user.Id, user.Email, cancellationToken: cancellationToken);
        return Success(result);
    }
}

