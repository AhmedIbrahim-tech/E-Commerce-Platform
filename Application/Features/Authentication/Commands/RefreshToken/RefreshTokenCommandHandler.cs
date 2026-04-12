using Application.Common.Bases;
using Application.Common.Constants;
using Application.Common.Errors;
using Application.Common.Helpers;
using Application.ServicesHandlers.Services;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Authentication.Commands.RefreshToken;

public class RefreshTokenCommandHandler(
    UserManager<AppUser> userManager,
    IAuthenticationService authenticationService,
    IUserStatusService userStatusService,
    IAuditService auditService) : ApiResponseHandler(),
    IRequestHandler<RefreshTokenCommand, ApiResponse<JwtAuthResponse>>
{
    public async Task<ApiResponse<JwtAuthResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var validation = await authenticationService.ValidateRefreshSessionAsync(request.RefreshToken, request.AccessToken);

        if (!validation.Success)
            return new ApiResponse<JwtAuthResponse>(UserErrors.InvalidRefreshToken());

        var user = await userManager.FindByIdAsync(validation.UserId.ToString());
        if (user == null)
        {
            _ = auditService.LogEventAsync(AuditEventType.Security, AuditEventName.RefreshTokenBlocked, "Refresh token attempt for non-existent user", validation.UserId, cancellationToken: cancellationToken);
            return new ApiResponse<JwtAuthResponse>(UserErrors.UserNotFound());
        }

        if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow)
        {
            _ = auditService.LogEventAsync(AuditEventType.Security, AuditEventName.RefreshTokenBlocked, "Refresh token attempt on locked account", user.Id, user.Email, cancellationToken: cancellationToken);
            return new ApiResponse<JwtAuthResponse>(UserErrors.LockedUser());
        }

        if (!user.EmailConfirmed)
        {
            _ = auditService.LogEventAsync(AuditEventType.Security, AuditEventName.RefreshTokenBlocked, "Refresh token attempt with unconfirmed email", user.Id, user.Email, cancellationToken: cancellationToken);
            return new ApiResponse<JwtAuthResponse>(UserErrors.EmailNotConfirmed());
        }

        if (await userStatusService.IsUserDeletedAsync(user.Id, cancellationToken))
        {
            _ = auditService.LogEventAsync(AuditEventType.Security, AuditEventName.RefreshTokenBlocked, "Refresh token attempt on disabled/deleted account", user.Id, user.Email, cancellationToken: cancellationToken);
            return new ApiResponse<JwtAuthResponse>(UserErrors.DisabledUser());
        }

        try
        {
            var result = await authenticationService.GetRefreshTokenAsync(user, validation.OldJwtId, request.RefreshToken);
            return Success(result);
        }
        catch (UnauthorizedAccessException)
        {
            return new ApiResponse<JwtAuthResponse>(UserErrors.InvalidRefreshToken());
        }
    }
}

