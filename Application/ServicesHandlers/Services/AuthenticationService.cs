namespace Application.ServicesHandlers.Services;

public interface IAuthenticationService
{
    Task<JwtAuthResponse> GetJWTTokenAsync(AppUser user);
    Task<JwtAuthResponse> GetRefreshTokenAsync(AppUser user, JwtSecurityToken jwtToken, DateTimeOffset? expiryDate, string refreshToken);
    JwtSecurityToken ReadJwtToken(string accessToken);
    Task<string> ValidateToken(string accessToken);
    Task<(string, DateTimeOffset?)> ValidateDetails(JwtSecurityToken jwtToken, string accessToken, string refreshToken);
    Task<string> ConfirmEmailAsync(Guid? userId, string? code);
    Task<string> SendResetPasswordCodeAsync(string email);
    Task<string> ConfirmResetPasswordAsync(string code, string email);
    Task<string> ResetPasswordAsync(string email, string newPassword);
    Common.Helpers.RefreshToken GetRefreshToken(string userName);
    Task<bool> LogoutAsync(Guid userId, string refreshToken);
    Task<bool> LogoutAllSessionsAsync(Guid userId);
    Task<int> CleanupExpiredTokensAsync(CancellationToken cancellationToken = default);
}

public class AuthenticationService : IAuthenticationService
{
    #region Fields
    private readonly UserManager<AppUser> _userManager;
    private readonly JwtSettings _jwtSettings;
    private readonly IEmailService _emailService;
    private readonly ApplicationDbContext _dbContext;
    private readonly IEncryptionProvider _encryptionProvider;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IAuditService _auditService;
    #endregion

    #region Constructors
    public AuthenticationService(UserManager<AppUser> userManager,
        IOptions<JwtSettings> jwtSettings,
        IEmailService emailService,
        ApplicationDbContext dbContext,
        IEncryptionProvider encryptionProvider,
        IRefreshTokenRepository refreshTokenRepository,
        IAuditService auditService)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
        _emailService = emailService;
        _dbContext = dbContext;
        _encryptionProvider = encryptionProvider;
        _refreshTokenRepository = refreshTokenRepository;
        _auditService = auditService;
    }
    #endregion

    #region Handle Functions
    public async Task<JwtAuthResponse> GetJWTTokenAsync(AppUser user)
    {
        var (jwtToken, accessToken) = await GenerateJwtToken(user);
        var refreshToken = GetRefreshToken(user.UserName!);
        var expiresAt = DateTimeOffset.UtcNow.AddDays(_jwtSettings.RefreshTokenExpireDate);
        
        // Store the actual refresh token string (not the access token)
        var userRefreshToken = new Infrastructure.Data.Identity.RefreshToken(
            appUserId: user.Id,
            token: refreshToken.TokenString,
            jwtId: jwtToken.Id,
            expiresAt: expiresAt
        );
        await _refreshTokenRepository.AddAsync(userRefreshToken, CancellationToken.None);
        await _refreshTokenRepository.SaveChangesAsync(CancellationToken.None);

        // Log successful login
        _ = _auditService.LogEventAsync(
            eventType: "Authentication",
            eventName: "UserLogin",
            description: $"User logged in successfully. Session created with JwtId {jwtToken.Id}",
            userId: user.Id,
            userEmail: user.Email,
            additionalData: $"{{\"JwtId\":\"{jwtToken.Id}\"}}"
        );

        var response = new JwtAuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

        return response;
    }

    private async Task<(JwtSecurityToken, string)> GenerateJwtToken(AppUser user)
    {
        var claims = await GetClaims(user);
        var jwtId = Guid.NewGuid().ToString();
        
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, jwtId));
        
        var jwtToken = new JwtSecurityToken(issuer: _jwtSettings.Issuer,
                                               audience: _jwtSettings.Audience,
                                               claims: claims,
                                               expires: DateTimeOffset.UtcNow.ToLocalTime().AddDays(_jwtSettings.AccessTokenExpireDate).DateTime,
                                               signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret!)), SecurityAlgorithms.HmacSha256Signature));
        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        return (jwtToken, accessToken);
    }

    private async Task<List<Claim>> GetClaims(AppUser user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new (ClaimTypes.Name, user.DisplayName!),
            new (ClaimTypes.Email, user.Email!),
            new (ClaimTypes.NameIdentifier, user.UserName!),
            new (nameof(UserClaimModel.Id), user.Id.ToString()),
            new (nameof(UserClaimModel.PhoneNumber), user.PhoneNumber ?? string.Empty),
            new ("ProfileImage", user.ProfileImage ?? string.Empty)
        };
        foreach (var role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        if (userRoles.Any())
        {
            claims.Add(new Claim("role", userRoles.First()));
        }
        var userClaims = await _userManager.GetClaimsAsync(user);
        claims.AddRange(userClaims);
        return claims;
    }

    public Common.Helpers.RefreshToken GetRefreshToken(string userName)
    {
        var refreshToken = new Common.Helpers.RefreshToken
        {
            UserName = userName,
            TokenString = GenerateRefreshToken(),
            ExpireAt = DateTimeOffset.UtcNow.ToLocalTime().AddDays(_jwtSettings.RefreshTokenExpireDate),
        };
        return refreshToken;
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    public async Task<JwtAuthResponse> GetRefreshTokenAsync(AppUser user, JwtSecurityToken jwtToken, DateTimeOffset? expiryDate, string refreshToken)
    {
        if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow)
        {
            _ = _auditService.LogEventAsync(
                eventType: "Security",
                eventName: "RefreshTokenBlocked",
                description: "Refresh token attempt on locked account",
                userId: user.Id,
                userEmail: user.Email
            );
            throw new UnauthorizedAccessException("Account is locked");
        }

        if (!user.EmailConfirmed)
        {
            _ = _auditService.LogEventAsync(
                eventType: "Security",
                eventName: "RefreshTokenBlocked",
                description: "Refresh token attempt with unconfirmed email",
                userId: user.Id,
                userEmail: user.Email
            );
            throw new UnauthorizedAccessException("Email not confirmed");
        }

        var userId = user.Id;
        var oldJwtId = jwtToken.Id;

        var oldRefreshToken = await _refreshTokenRepository.GetTableAsTracking()
            .FirstOrDefaultAsync(x => x.Token == refreshToken && 
                                 x.JwtId == oldJwtId && 
                                 x.AppUserId == userId);
        
        if (oldRefreshToken != null)
        {
            oldRefreshToken.MarkAsUsed();
            await _refreshTokenRepository.UpdateAsync(oldRefreshToken, CancellationToken.None);
        }

        var (jwtSecurityToken, newToken) = await GenerateJwtToken(user);
        var newRefreshToken = GetRefreshToken(user.UserName!);
        var newExpiresAt = DateTimeOffset.UtcNow.AddDays(_jwtSettings.RefreshTokenExpireDate);

        var userRefreshToken = new Infrastructure.Data.Identity.RefreshToken(
            appUserId: user.Id,
            token: newRefreshToken.TokenString,
            jwtId: jwtSecurityToken.Id,
            expiresAt: newExpiresAt
        );
        await _refreshTokenRepository.AddAsync(userRefreshToken, CancellationToken.None);
        await _refreshTokenRepository.SaveChangesAsync(CancellationToken.None);

        _ = _auditService.LogEventAsync(
            eventType: "Authentication",
            eventName: "RefreshTokenRotated",
            description: $"Refresh token rotated. Old JwtId: {oldJwtId}, New JwtId: {jwtSecurityToken.Id}",
            userId: userId,
            userEmail: user.Email,
            additionalData: $"{{\"OldJwtId\":\"{oldJwtId}\",\"NewJwtId\":\"{jwtSecurityToken.Id}\"}}"
        );

        var response = new JwtAuthResponse
        {
            AccessToken = newToken,
            RefreshToken = newRefreshToken
        };

        return response;
    }

    public JwtSecurityToken ReadJwtToken(string accessToken)
    {
        if (string.IsNullOrEmpty(accessToken))
            throw new ArgumentNullException(nameof(accessToken), "Access token cannot be null or empty.");
        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(accessToken))
            throw new ArgumentException("Invalid JWT token.", nameof(accessToken));
        var response = handler.ReadJwtToken(accessToken);
        return response;
    }

    public async Task<string> ValidateToken(string accessToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var parameters = new TokenValidationParameters
        {
            ValidateIssuer = _jwtSettings.ValidateIssuer,
            ValidIssuers = new[] { _jwtSettings.Issuer },
            ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret!)),
            ValidAudience = _jwtSettings.Audience,
            ValidateAudience = _jwtSettings.ValidateAudience,
            ValidateLifetime = _jwtSettings.ValidateLifeTime,
        };
        try
        {
            var validator = handler.ValidateToken(accessToken, parameters, out SecurityToken validatedToken);

            if (validator == null)
                return "Invalid JWT token.";

            return "Not Expired.";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public async Task<(string, DateTimeOffset?)> ValidateDetails(JwtSecurityToken jwtToken, string accessToken, string refreshToken)
    {
        if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
            return ("AlgorithmIsWrong", null);
        if (jwtToken.ValidTo > DateTimeOffset.UtcNow.ToLocalTime())
            return ("TokenIsNotExpired", null);

        var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == nameof(UserClaimModel.Id))!.Value;
        
        // Find refresh token by matching the actual refresh token string (not accessToken)
        var userRefreshToken = await _refreshTokenRepository.GetTableAsTracking()
                                                            .FirstOrDefaultAsync(x => x.Token == refreshToken &&
                                                                                x.JwtId == jwtToken.Id &&
                                                                                x.AppUserId == Guid.Parse(userId));
        if (userRefreshToken == null)
        {
            _ = _auditService.LogEventAsync(
                eventType: "Security",
                eventName: "RefreshTokenNotFound",
                description: $"Refresh token not found for user with JwtId {jwtToken.Id}",
                userId: Guid.Parse(userId),
                additionalData: $"{{\"JwtId\":\"{jwtToken.Id}\"}}"
            );
            return ("RefreshTokenIsNotFound", null);
        }

        // Check if token is already used (replay attack detection)
        if (userRefreshToken.IsUsed)
        {
            _ = _auditService.LogEventAsync(
                eventType: "Security",
                eventName: "RefreshTokenReused",
                description: $"Reused refresh token detected. Possible replay attack.",
                userId: Guid.Parse(userId),
                additionalData: $"{{\"JwtId\":\"{jwtToken.Id}\"}}"
            );
            // Revoke all tokens for this user as a security measure
            await RevokeAllUserTokensAsync(Guid.Parse(userId), "Reused refresh token detected");
            return ("RefreshTokenReused", null);
        }

        // Check if token is revoked
        if (userRefreshToken.IsRevoked)
        {
            _ = _auditService.LogEventAsync(
                eventType: "Security",
                eventName: "RevokedRefreshTokenUsed",
                description: $"Revoked refresh token used for user with JwtId {jwtToken.Id}",
                userId: Guid.Parse(userId),
                additionalData: $"{{\"JwtId\":\"{jwtToken.Id}\"}}"
            );
            return ("RefreshTokenRevoked", null);
        }

        // Check if token is expired
        if (userRefreshToken.ExpiresAt < DateTimeOffset.UtcNow)
        {
            userRefreshToken.Revoke("Token expired");
            await _refreshTokenRepository.UpdateAsync(userRefreshToken, CancellationToken.None);
            await _refreshTokenRepository.SaveChangesAsync(CancellationToken.None);
            _ = _auditService.LogEventAsync(
                eventType: "Authentication",
                eventName: "RefreshTokenExpired",
                description: $"Expired refresh token revoked",
                userId: Guid.Parse(userId),
                additionalData: $"{{\"JwtId\":\"{jwtToken.Id}\"}}"
            );
            return ("RefreshTokenIsExpired", null);
        }

        var expirydate = userRefreshToken.ExpiresAt;
        return (userId, expirydate);
    }

    public async Task<string> ConfirmEmailAsync(Guid? userId, string? code)
    {
        if (userId is null || string.IsNullOrEmpty(code))
            return "UserOrCodeIsNullOrEmpty";
        var user = await _userManager.FindByIdAsync(userId.ToString()!);
        var confirmEmailResult = await _userManager.ConfirmEmailAsync(user!, code);
        if (!confirmEmailResult.Succeeded)
            return string.Join(",", confirmEmailResult.Errors.Select(x => x.Description).ToList());
        return "Success";
    }

    public async Task<string> SendResetPasswordCodeAsync(string email)
    {
        var trans = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return "UserNotFound";

            var chars = "0123456789";
            var random = new Random();
            var randomNumber = new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());

            user.Code = randomNumber;
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return "ErrorInUpdateUser";

            var resetPasswordBody = BuildResetPasswordEmailBody(user.Code);
            var emailDto = new EmailDto
            {
                MailTo = user.Email!,
                Subject = "Reset Password",
                Body = resetPasswordBody
            };
            await _emailService.SendEmailsAsync(emailDto);
            await trans.CommitAsync();
            return "Success";
        }
        catch (Exception)
        {
            await trans.RollbackAsync();
            return "Failed";
        }
    }

    public async Task<string> ConfirmResetPasswordAsync(string code, string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null) return "UserNotFound";

        var userCode = user.Code;

        if (userCode == code) return "Success";
        return "Failed";
    }

    public async Task<string> ResetPasswordAsync(string email, string newNassword)
    {
        var trans = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return "UserNotFound";

            await _userManager.RemovePasswordAsync(user);
            if (!await _userManager.HasPasswordAsync(user))
                await _userManager.AddPasswordAsync(user, newNassword);

            await trans.CommitAsync();
            return "Success";
        }
        catch (Exception)
        {
            await trans.RollbackAsync();
            return "Failed";
        }
    }

    public async Task<bool> LogoutAsync(Guid userId, string refreshToken)
    {
        try
        {
            var userRefreshToken = await _refreshTokenRepository.GetTableAsTracking()
                .FirstOrDefaultAsync(x => x.Token == refreshToken && x.AppUserId == userId);

            if (userRefreshToken != null && userRefreshToken.IsActive())
            {
                userRefreshToken.Revoke("User logout");
                await _refreshTokenRepository.UpdateAsync(userRefreshToken, CancellationToken.None);
                await _refreshTokenRepository.SaveChangesAsync(CancellationToken.None);
                
                _ = _auditService.LogEventAsync(
                    eventType: "Authentication",
                    eventName: "UserLogout",
                    description: $"User logged out. Session revoked with JwtId {userRefreshToken.JwtId}",
                    userId: userId,
                    additionalData: $"{{\"JwtId\":\"{userRefreshToken.JwtId}\"}}"
                );
                return true;
            }

            _ = _auditService.LogEventAsync(
                eventType: "Security",
                eventName: "InvalidLogoutAttempt",
                description: "Logout attempted with invalid refresh token",
                userId: userId
            );
            return false;
        }
        catch (Exception ex)
        {
            _ = _auditService.LogEventAsync(
                eventType: "Error",
                eventName: "LogoutError",
                description: $"Error during logout: {ex.Message}",
                userId: userId
            );
            return false;
        }
    }

    public async Task<bool> LogoutAllSessionsAsync(Guid userId)
    {
        try
        {
            var activeTokens = await _refreshTokenRepository.GetTableAsTracking()
                .Where(x => x.AppUserId == userId && x.IsActive())
                .ToListAsync();

            foreach (var token in activeTokens)
            {
                token.Revoke("User logout all sessions");
            }

            if (activeTokens.Any())
            {
                await _refreshTokenRepository.SaveChangesAsync(CancellationToken.None);
                _ = _auditService.LogEventAsync(
                    eventType: "Authentication",
                    eventName: "UserLogoutAll",
                    description: $"User logged out from all sessions. {activeTokens.Count} sessions revoked",
                    userId: userId,
                    additionalData: $"{{\"SessionsRevoked\":{activeTokens.Count}}}"
                );
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _ = _auditService.LogEventAsync(
                eventType: "Error",
                eventName: "LogoutAllError",
                description: $"Error during logout all sessions: {ex.Message}",
                userId: userId
            );
            return false;
        }
    }

    private async Task RevokeAllUserTokensAsync(Guid userId, string reason)
    {
        var activeTokens = await _refreshTokenRepository.GetTableAsTracking()
            .Where(x => x.AppUserId == userId && x.IsActive())
            .ToListAsync();

        foreach (var token in activeTokens)
        {
            token.Revoke(reason);
        }

        if (activeTokens.Any())
        {
            await _refreshTokenRepository.SaveChangesAsync(CancellationToken.None);
        }
    }

    public async Task<int> CleanupExpiredTokensAsync(CancellationToken cancellationToken = default)
    {
        var expiredTokens = await _refreshTokenRepository.GetTableAsTracking()
            .Where(x => x.ExpiresAt < DateTimeOffset.UtcNow && x.IsActive())
            .ToListAsync(cancellationToken);

        foreach (var token in expiredTokens)
        {
            token.Revoke("Token expired - cleanup job");
        }

        if (expiredTokens.Any())
        {
            await _refreshTokenRepository.SaveChangesAsync(cancellationToken);
        }

        return expiredTokens.Count;
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
    #endregion
}

