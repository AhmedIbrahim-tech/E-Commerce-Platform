namespace Application.ServicesHandlers.Services;

public interface IAuthenticationService
{
    Task<JwtAuthResponse> GetJWTTokenAsync(AppUser user, bool rememberMe = false);
    Task<JwtAuthResponse> GetRefreshTokenAsync(AppUser user, JwtSecurityToken jwtToken, DateTimeOffset? expiryDate, string refreshToken);
    JwtSecurityToken ReadJwtToken(string accessToken);
    Task<(bool IsValid, string Message)> ValidateToken(string accessToken);
    Task<(string, DateTimeOffset?)> ValidateDetails(JwtSecurityToken jwtToken, string accessToken, string refreshToken);
    Task<string> ConfirmEmailAsync(Guid? userId, string? code);
    Task<string> SendResetPasswordCodeAsync(string email);
    Task<string> ConfirmResetPasswordAsync(string code, string email);
    Task<string> ResetPasswordAsync(string email, string newPassword);
    Common.Helpers.RefreshToken GetRefreshToken(string userName, bool rememberMe = false);
    Task<bool> LogoutAsync(Guid userId, string refreshToken);
    Task<bool> LogoutAllSessionsAsync(Guid userId);
    Task<int> CleanupExpiredTokensAsync(CancellationToken cancellationToken = default);
}

public class AuthenticationService(UserManager<AppUser> userManager, IOptions<JwtSettings> jwtSettings, IEmailService emailService, IUnitOfWork unitOfWork, IRefreshTokenRepository refreshTokenRepository, IAuditService auditService) : IAuthenticationService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public async Task<JwtAuthResponse> GetJWTTokenAsync(AppUser user, bool rememberMe = false)
    {
        var (jwtToken, accessToken) = await GenerateJwtToken(user);
        var refreshToken = GetRefreshToken(user.UserName!, rememberMe);
        var refreshDays = rememberMe ? _jwtSettings.RefreshTokenExpireDateRememberMe : _jwtSettings.RefreshTokenExpireDate;
        var expiresAt = DateTimeOffset.UtcNow.AddDays(refreshDays);

        var userRefreshToken = new Infrastructure.Data.Identity.RefreshToken(
            user.Id,
            refreshToken.TokenString,
            jwtToken.Id,
            expiresAt);
        await refreshTokenRepository.AddAsync(userRefreshToken, CancellationToken.None);
        await refreshTokenRepository.SaveChangesAsync(CancellationToken.None);

        await auditService.LogEventAsync(
            AuditEventType.Authentication,
            AuditEventName.UserLogin,
            $"User logged in successfully. Session created with JwtId {jwtToken.Id}",
            user.Id,
            user.Email,
            $"{{\"JwtId\":\"{jwtToken.Id}\"}}");

        var roles = await userManager.GetRolesAsync(user);
        return new JwtAuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Roles = roles.ToList()
        };
    }

    public async Task<JwtAuthResponse> GetRefreshTokenAsync(AppUser user, JwtSecurityToken jwtToken, DateTimeOffset? expiryDate, string refreshToken)
    {
        if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow)
        {
            await auditService.LogEventAsync(AuditEventType.Security, AuditEventName.RefreshTokenBlocked, "Refresh token attempt on locked account", user.Id, user.Email);
            throw new UnauthorizedAccessException("Account is locked");
        }

        if (!user.EmailConfirmed)
        {
            await auditService.LogEventAsync(AuditEventType.Security, AuditEventName.RefreshTokenBlocked, "Refresh token attempt with unconfirmed email", user.Id, user.Email);
            throw new UnauthorizedAccessException("Email not confirmed");
        }

        var oldJwtId = jwtToken.Id;
        var oldRefreshToken = await refreshTokenRepository.GetTableAsTracking()
            .FirstOrDefaultAsync(x => x.Token == refreshToken && x.JwtId == oldJwtId && x.AppUserId == user.Id);

        if (oldRefreshToken != null)
        {
            oldRefreshToken.MarkAsUsed();
            await refreshTokenRepository.UpdateAsync(oldRefreshToken, CancellationToken.None);
        }

        var (jwtSecurityToken, newToken) = await GenerateJwtToken(user);
        var newRefreshToken = GetRefreshToken(user.UserName!);
        var newExpiresAt = DateTimeOffset.UtcNow.AddDays(_jwtSettings.RefreshTokenExpireDate);

        var userRefreshToken = new Infrastructure.Data.Identity.RefreshToken(
            user.Id,
            newRefreshToken.TokenString,
            jwtSecurityToken.Id,
            newExpiresAt);
        await refreshTokenRepository.AddAsync(userRefreshToken, CancellationToken.None);
        await refreshTokenRepository.SaveChangesAsync(CancellationToken.None);

        await auditService.LogEventAsync(
            AuditEventType.Authentication,
            AuditEventName.RefreshTokenRotated,
            $"Refresh token rotated. Old JwtId: {oldJwtId}, New JwtId: {jwtSecurityToken.Id}",
            user.Id,
            user.Email,
            $"{{\"OldJwtId\":\"{oldJwtId}\",\"NewJwtId\":\"{jwtSecurityToken.Id}\"}}");

        var roles = await userManager.GetRolesAsync(user);
        return new JwtAuthResponse
        {
            AccessToken = newToken,
            RefreshToken = newRefreshToken,
            Roles = roles.ToList()
        };
    }

    public JwtSecurityToken ReadJwtToken(string accessToken)
    {
        if (string.IsNullOrEmpty(accessToken))
            throw new ArgumentNullException(nameof(accessToken), "Access token cannot be null or empty.");
        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(accessToken))
            throw new ArgumentException("Invalid JWT token.", nameof(accessToken));
        return handler.ReadJwtToken(accessToken);
    }

    public async Task<(bool IsValid, string Message)> ValidateToken(string accessToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var parameters = BuildTokenValidationParameters();
        try
        {
            var principal = handler.ValidateToken(accessToken, parameters, out _);
            return principal == null ? (false, "Invalid JWT token.") : (true, AuthenticationResult.Success);
        }
        catch (SecurityTokenExpiredException)
        {
            return (false, "Token has expired.");
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public async Task<(string, DateTimeOffset?)> ValidateDetails(JwtSecurityToken jwtToken, string accessToken, string refreshToken)
    {
        if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
            return ("AlgorithmIsWrong", null);
        if (jwtToken.ValidTo > DateTimeOffset.UtcNow.ToLocalTime())
            return ("TokenIsNotExpired", null);

        var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == nameof(UserClaimModel.Id))!.Value;
        var userRefreshToken = await refreshTokenRepository.GetTableAsTracking()
            .FirstOrDefaultAsync(x => x.Token == refreshToken && x.JwtId == jwtToken.Id && x.AppUserId == Guid.Parse(userId));

        if (userRefreshToken == null)
        {
            await auditService.LogEventAsync(AuditEventType.Security, AuditEventName.RefreshTokenNotFound, $"Refresh token not found for user with JwtId {jwtToken.Id}", Guid.Parse(userId), additionalData: $"{{\"JwtId\":\"{jwtToken.Id}\"}}");
            return ("RefreshTokenIsNotFound", null);
        }

        if (userRefreshToken.IsUsed)
        {
            await auditService.LogEventAsync(AuditEventType.Security, AuditEventName.RefreshTokenReused, "Reused refresh token detected. Possible replay attack.", Guid.Parse(userId), additionalData: $"{{\"JwtId\":\"{jwtToken.Id}\"}}");
            await RevokeAllUserTokensAsync(Guid.Parse(userId), "Reused refresh token detected");
            return ("RefreshTokenReused", null);
        }

        if (userRefreshToken.IsRevoked)
        {
            await auditService.LogEventAsync(AuditEventType.Security, AuditEventName.RevokedRefreshTokenUsed, $"Revoked refresh token used for user with JwtId {jwtToken.Id}", Guid.Parse(userId), additionalData: $"{{\"JwtId\":\"{jwtToken.Id}\"}}");
            return ("RefreshTokenRevoked", null);
        }

        if (userRefreshToken.ExpiresAt < DateTimeOffset.UtcNow)
        {
            userRefreshToken.Revoke("Token expired");
            await refreshTokenRepository.UpdateAsync(userRefreshToken, CancellationToken.None);
            await refreshTokenRepository.SaveChangesAsync(CancellationToken.None);
            await auditService.LogEventAsync(AuditEventType.Authentication, AuditEventName.RefreshTokenExpired, "Expired refresh token revoked", Guid.Parse(userId), additionalData: $"{{\"JwtId\":\"{jwtToken.Id}\"}}");
            return ("RefreshTokenIsExpired", null);
        }

        return (userId, userRefreshToken.ExpiresAt);
    }

    public async Task<string> ConfirmEmailAsync(Guid? userId, string? code)
    {
        if (userId is null || string.IsNullOrEmpty(code))
            return AuthenticationResult.UserOrCodeIsNullOrEmpty;
        var user = await userManager.FindByIdAsync(userId.ToString()!);
        var confirmEmailResult = await userManager.ConfirmEmailAsync(user!, code);
        if (!confirmEmailResult.Succeeded)
            return string.Join(",", confirmEmailResult.Errors.Select(x => x.Description).ToList());
        return AuthenticationResult.Success;
    }

    public async Task<string> SendResetPasswordCodeAsync(string email)
    {
        using var trans = await unitOfWork.BeginTransactionAsync(CancellationToken.None);
        try
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null) return AuthenticationResult.UserNotFound;

            user.Code = GenerateNumericCode(6);
            var updateResult = await userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                await unitOfWork.RollbackTransactionAsync(CancellationToken.None);
                return AuthenticationResult.ErrorInUpdateUser;
            }

            var emailDto = new EmailDto
            {
                MailTo = user.Email!,
                Subject = "Reset Password",
                Body = BuildResetPasswordEmailBody(user.Code)
            };
            await emailService.SendEmailsAsync(emailDto);
            await unitOfWork.CommitTransactionAsync(CancellationToken.None);
            return AuthenticationResult.Success;
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync(CancellationToken.None);
            return AuthenticationResult.Failed;
        }
    }

    public async Task<string> ConfirmResetPasswordAsync(string code, string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null) return AuthenticationResult.UserNotFound;
        return user.Code == code ? AuthenticationResult.Success : AuthenticationResult.Failed;
    }

    public async Task<string> ResetPasswordAsync(string email, string newPassword)
    {
        using var trans = await unitOfWork.BeginTransactionAsync(CancellationToken.None);
        try
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null) return AuthenticationResult.UserNotFound;

            await userManager.RemovePasswordAsync(user);
            if (!await userManager.HasPasswordAsync(user))
                await userManager.AddPasswordAsync(user, newPassword);

            user.Code = null;
            var updateResult = await userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                await unitOfWork.RollbackTransactionAsync(CancellationToken.None);
                return AuthenticationResult.ErrorInUpdateUser;
            }

            await unitOfWork.CommitTransactionAsync(CancellationToken.None);
            return AuthenticationResult.Success;
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync(CancellationToken.None);
            return AuthenticationResult.Failed;
        }
    }

    public Common.Helpers.RefreshToken GetRefreshToken(string userName, bool rememberMe = false)
    {
        var refreshDays = rememberMe ? _jwtSettings.RefreshTokenExpireDateRememberMe : _jwtSettings.RefreshTokenExpireDate;
        return new Common.Helpers.RefreshToken
        {
            UserName = userName,
            TokenString = GenerateRefreshToken(),
            ExpireAt = DateTimeOffset.UtcNow.ToLocalTime().AddDays(refreshDays)
        };
    }

    public async Task<bool> LogoutAsync(Guid userId, string refreshToken)
    {
        try
        {
            var userRefreshToken = await refreshTokenRepository.GetTableAsTracking()
                .FirstOrDefaultAsync(x => x.Token == refreshToken && x.AppUserId == userId);

            if (userRefreshToken != null && userRefreshToken.IsActive())
            {
                userRefreshToken.Revoke("User logout");
                await refreshTokenRepository.UpdateAsync(userRefreshToken, CancellationToken.None);
                await refreshTokenRepository.SaveChangesAsync(CancellationToken.None);
                await auditService.LogEventAsync(AuditEventType.Authentication, AuditEventName.UserLogout, $"User logged out. Session revoked with JwtId {userRefreshToken.JwtId}", userId, additionalData: $"{{\"JwtId\":\"{userRefreshToken.JwtId}\"}}");
                return true;
            }

            await auditService.LogEventAsync(AuditEventType.Security, AuditEventName.InvalidLogoutAttempt, "Logout attempted with invalid refresh token", userId);
            return false;
        }
        catch (Exception ex)
        {
            await auditService.LogEventAsync(AuditEventType.Error, AuditEventName.LogoutError, $"Error during logout: {ex.Message}", userId);
            return false;
        }
    }

    public async Task<bool> LogoutAllSessionsAsync(Guid userId)
    {
        try
        {
            var activeTokens = await refreshTokenRepository.GetTableAsTracking()
                .Where(x => x.AppUserId == userId && x.IsActive())
                .ToListAsync();

            foreach (var token in activeTokens)
                token.Revoke("User logout all sessions");

            if (activeTokens.Count != 0)
            {
                await refreshTokenRepository.SaveChangesAsync(CancellationToken.None);
                await auditService.LogEventAsync(AuditEventType.Authentication, AuditEventName.UserLogoutAll, $"User logged out from all sessions. {activeTokens.Count} sessions revoked", userId, additionalData: $"{{\"SessionsRevoked\":{activeTokens.Count}}}");
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            await auditService.LogEventAsync(AuditEventType.Error, AuditEventName.LogoutAllError, $"Error during logout all sessions: {ex.Message}", userId);
            return false;
        }
    }

    public async Task<int> CleanupExpiredTokensAsync(CancellationToken cancellationToken = default)
    {
        var expiredTokens = await refreshTokenRepository.GetTableAsTracking()
            .Where(x => x.ExpiresAt < DateTimeOffset.UtcNow && x.IsActive())
            .ToListAsync(cancellationToken);

        foreach (var token in expiredTokens)
            token.Revoke("Token expired - cleanup job");

        if (expiredTokens.Count != 0)
            await refreshTokenRepository.SaveChangesAsync(cancellationToken);

        return expiredTokens.Count;
    }

    private async Task<(JwtSecurityToken, string)> GenerateJwtToken(AppUser user)
    {
        var claims = await GetClaims(user);
        var jwtId = Guid.NewGuid().ToString();
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, jwtId));

        var jwtToken = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            DateTimeOffset.UtcNow.ToLocalTime().AddDays(_jwtSettings.AccessTokenExpireDate).DateTime,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret!)), SecurityAlgorithms.HmacSha256Signature));
        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        return (jwtToken, accessToken);
    }

    private async Task<List<Claim>> GetClaims(AppUser user)
    {
        var userRoles = await userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.DisplayName!),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.NameIdentifier, user.UserName!),
            new(nameof(UserClaimModel.Id), user.Id.ToString()),
            new(nameof(UserClaimModel.PhoneNumber), user.PhoneNumber ?? string.Empty),
            new("ProfileImage", user.ProfileImage ?? string.Empty)
        };
        foreach (var role in userRoles)
            claims.Add(new Claim(ClaimTypes.Role, role));
        if (userRoles.Any())
            claims.Add(new Claim("role", userRoles.First()));
        var userClaims = await userManager.GetClaimsAsync(user);
        claims.AddRange(userClaims);
        return claims;
    }

    private TokenValidationParameters BuildTokenValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = _jwtSettings.ValidateIssuer,
            ValidIssuers = new[] { _jwtSettings.Issuer },
            ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret!)),
            ValidAudience = _jwtSettings.Audience,
            ValidateAudience = _jwtSettings.ValidateAudience,
            ValidateLifetime = _jwtSettings.ValidateLifeTime,
            ClockSkew = TimeSpan.Zero
        };
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private static string GenerateNumericCode(int length)
    {
        const string chars = "0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private async Task RevokeAllUserTokensAsync(Guid userId, string reason)
    {
        var activeTokens = await refreshTokenRepository.GetTableAsTracking()
            .Where(x => x.AppUserId == userId && x.IsActive())
            .ToListAsync();

        foreach (var token in activeTokens)
            token.Revoke(reason);

        if (activeTokens.Count != 0)
            await refreshTokenRepository.SaveChangesAsync(CancellationToken.None);
    }

    private static string BuildResetPasswordEmailBody(string code)
    {
        return $@"
            <div style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px; text-align: center;'>
                <div style='max-width: 600px; background: white; padding: 30px; border-radius: 10px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); margin: auto;'>
                    <h2 style='color: #333;'>Reset Your Password</h2>
                    <p style='color: #555; font-size: 16px;'>
                        We received a request to reset your password. Use the code below to proceed.
                    </p>
                    <div style='display: inline-block; background-color: #007bff; color: white; padding: 12px 20px; border-radius: 5px; font-size: 20px; margin-top: 20px; font-weight: bold;'>{code}</div>
                    <p style='color: #999; font-size: 14px; margin-top: 20px;'>
                        If you didn't request this, you can safely ignore this email.
                    </p>
                </div>
            </div>";
    }
}
