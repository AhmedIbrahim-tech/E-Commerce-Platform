namespace Application.Common.Settings;

public class JwtSettings
{
    public string? Secret { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public bool ValidateIssuer { get; set; }
    public bool ValidateAudience { get; set; }
    public bool ValidateLifeTime { get; set; }
    public bool ValidateIssuerSigningKey { get; set; }

    /// <summary>Access token lifetime in whole days (UTC). Prefer short values (e.g. 1) in production.</summary>
    public int AccessTokenExpireDate { get; set; }

    public int RefreshTokenExpireDate { get; set; }
    public int RefreshTokenExpireDateRememberMe { get; set; } = 30;
}
