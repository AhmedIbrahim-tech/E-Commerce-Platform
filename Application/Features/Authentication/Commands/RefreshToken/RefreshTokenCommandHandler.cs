using Application.Common.Helpers;
using Application.ServicesHandlers.Services;
using Infrastructure.Data;

namespace Application.Features.Authentication.Commands.RefreshToken;

public class RefreshTokenCommandHandler(
    UserManager<AppUser> userManager,
    IAuthenticationService authenticationService,
    ApplicationDbContext dbContext,
    IAuditService auditService) : ApiResponseHandler(),
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
            case ("RefreshTokenReused", null): return new ApiResponse<JwtAuthResponse>(UserErrors.InvalidRefreshToken());
            case ("RefreshTokenRevoked", null): return new ApiResponse<JwtAuthResponse>(UserErrors.InvalidRefreshToken());
        }
        var (userId, expiryDate) = userIdAndExpireDate;
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            _ = auditService.LogEventAsync(
                eventType: "Security",
                eventName: "RefreshTokenBlocked",
                description: "Refresh token attempt for non-existent user",
                userId: Guid.Parse(userId),
                cancellationToken: cancellationToken
            );
            return new ApiResponse<JwtAuthResponse>(UserErrors.UserNotFound());
        }

        if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow)
        {
            _ = auditService.LogEventAsync(
                eventType: "Security",
                eventName: "RefreshTokenBlocked",
                description: "Refresh token attempt on locked account",
                userId: user.Id,
                userEmail: user.Email,
                cancellationToken: cancellationToken
            );
            return new ApiResponse<JwtAuthResponse>(UserErrors.LockedUser());
        }

        if (!user.EmailConfirmed)
        {
            _ = auditService.LogEventAsync(
                eventType: "Security",
                eventName: "RefreshTokenBlocked",
                description: "Refresh token attempt with unconfirmed email",
                userId: user.Id,
                userEmail: user.Email,
                cancellationToken: cancellationToken
            );
            return new ApiResponse<JwtAuthResponse>(UserErrors.EmailNotConfirmed());
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
                eventName: "RefreshTokenBlocked",
                description: "Refresh token attempt on disabled/deleted account",
                userId: user.Id,
                userEmail: user.Email,
                cancellationToken: cancellationToken
            );
            return new ApiResponse<JwtAuthResponse>(UserErrors.DisabledUser());
        }

        try
        {
            var result = await authenticationService.GetRefreshTokenAsync(user, jwtToken, expiryDate, request.RefreshToken);
            return Success(result);
        }
        catch (UnauthorizedAccessException)
        {
            return new ApiResponse<JwtAuthResponse>(UserErrors.InvalidRefreshToken());
        }
    }
}

