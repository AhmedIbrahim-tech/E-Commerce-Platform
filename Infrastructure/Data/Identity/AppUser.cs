namespace Infrastructure.Data.Identity;

public class AppUser : IdentityUser<Guid>
{
    // Display name shown in UI (reviews, orders, etc.)
    public string DisplayName { get; private set; } = null!;

    // Reset password code
    public string? Code { get; set; }

    // Profile image URL
    public string? ProfileImage { get; set; }

    // Sessions / Refresh Tokens
    private readonly List<RefreshToken> _refreshTokens = [];
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

    private AppUser() { } // EF

    public AppUser(string userName, string displayName)
    {
        UserName = userName;
        SetDisplayName(displayName);
    }

    public void SetDisplayName(string displayName)
    {
        if (string.IsNullOrWhiteSpace(displayName))
            throw new ArgumentException("Display name is required");

        DisplayName = Normalize(displayName);
    }

    public void AddRefreshToken(RefreshToken token)
    {
        _refreshTokens.Add(token);
    }

    public void RevokeRefreshToken(string token, string reason)
    {
        var refreshToken = _refreshTokens.SingleOrDefault(x => x.Token == token);
        refreshToken?.Revoke(reason);
    }

    private static string Normalize(string value)
        => System.Text.RegularExpressions.Regex.Replace(value.Trim(), @"\s+", " ");

}
