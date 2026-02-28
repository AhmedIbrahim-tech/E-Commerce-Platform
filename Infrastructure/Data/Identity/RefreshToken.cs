namespace Infrastructure.Data.Identity;

public class RefreshToken
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid AppUserId { get; private set; }
    public AppUser AppUser { get; private set; } = null!;
    public string Token { get; private set; } = null!;
    public string JwtId { get; private set; } = null!;

    public bool IsUsed { get; private set; }
    public bool IsRevoked { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset ExpiresAt { get; private set; }

    public string? RevokedReason { get; private set; }
    public DateTimeOffset? RevokedAt { get; private set; }

    private RefreshToken() { }

    public RefreshToken(Guid appUserId, string token, string jwtId, DateTimeOffset expiresAt)
    {
        AppUserId = appUserId;
        Token = token;
        JwtId = jwtId;
        ExpiresAt = expiresAt;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public bool IsActive() => !IsUsed && !IsRevoked && DateTimeOffset.UtcNow <= ExpiresAt;

    public void MarkAsUsed()
    {
        IsUsed = true;
    }

    public void Revoke(string reason)
    {
        IsRevoked = true;
        RevokedReason = reason;
        RevokedAt = DateTimeOffset.UtcNow;
    }
}
