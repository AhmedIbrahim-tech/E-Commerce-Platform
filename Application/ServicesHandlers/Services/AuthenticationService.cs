using Application.Common.Settings;
using Infrastructure.Data.Identity;

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
    RefreshToken GetRefreshToken(string userName);
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
    #endregion

    #region Constructors
    public AuthenticationService(UserManager<AppUser> userManager,
        IOptions<JwtSettings> jwtSettings,
        IEmailService emailService,
        ApplicationDbContext dbContext,
        IEncryptionProvider encryptionProvider,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
        _emailService = emailService;
        _dbContext = dbContext;
        _encryptionProvider = new GenerateEncryptionProvider("8a4dcaaec64d412380fe4b02193cd26f");
        _refreshTokenRepository = refreshTokenRepository;
    }
    #endregion

    #region Handle Functions
    public async Task<JwtAuthResponse> GetJWTTokenAsync(AppUser user)
    {
        var (jwtToken, accessToken) = await GenerateJwtToken(user);
        var refreshToken = GetRefreshToken(user.UserName!);
        var userRefreshToken = new UserRefreshToken
        {
            IsUsed = true,
            IsRevoked = false,
            ExpiryDate = DateTimeOffset.Now.ToLocalTime().AddDays(_jwtSettings.RefreshTokenExpireDate),
            AddedTime = DateTimeOffset.Now.ToLocalTime(),
            JwtId = jwtToken.Id,
            RefreshToken = refreshToken.TokenString,
            Token = accessToken,
            UserId = user.Id
        };
        await _refreshTokenRepository.AddAsync(userRefreshToken);

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
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.NameIdentifier, user.UserName!),
            new Claim(nameof(UserClaimModel.Id), user.Id.ToString()),
            new Claim(nameof(UserClaimModel.PhoneNumber), user.PhoneNumber!)
        };
        foreach (var role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        var userClaims = await _userManager.GetClaimsAsync(user);
        claims.AddRange(userClaims);
        return claims;
    }

    public RefreshToken GetRefreshToken(string userName)
    {
        var refreshToken = new RefreshToken
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
        var (jwtSecurityToken, newToken) = await GenerateJwtToken(user);
        var response = new JwtAuthResponse();
        response.AccessToken = newToken;

        var refreshTokenResult = new RefreshToken();
        var userNameClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == nameof(UserClaimModel.UserName));
        refreshTokenResult.UserName = userNameClaim?.Value ?? user.UserName!;
        refreshTokenResult.TokenString = refreshToken;
        refreshTokenResult.ExpireAt = (DateTimeOffset)expiryDate!;

        response.RefreshToken = refreshTokenResult;
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
        var userRefreshToken = await _refreshTokenRepository.GetTableNoTracking()
                                                            .FirstOrDefaultAsync(x => x.Token == accessToken &&
                                                                                x.RefreshToken == refreshToken &&
                                                                                x.UserId == Guid.Parse(userId));
        if (userRefreshToken == null)
            return ("RefreshTokenIsNotFound", null);

        if (userRefreshToken.ExpiryDate < DateTimeOffset.UtcNow.ToLocalTime())
        {
            userRefreshToken.IsRevoked = true;
            userRefreshToken.IsUsed = false;
            await _refreshTokenRepository.UpdateAsync(userRefreshToken);
            return ("RefreshTokenIsExpired", null);
        }
        var expirydate = userRefreshToken.ExpiryDate;
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

            var sendEmailResult = await _emailService.SendEmailAsync(user.Email!, user.Code, EmailType.ResetPassword, null);
            if (sendEmailResult == "Failed") return "SendEmailFailed";
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
    #endregion
}

