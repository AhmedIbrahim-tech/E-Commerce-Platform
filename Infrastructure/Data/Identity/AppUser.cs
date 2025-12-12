using EntityFrameworkCore.EncryptColumn.Attribute;

namespace Infrastructure.Data.Identity;

public class AppUser : IdentityUser<Guid>
{
    [EncryptColumn]
    public string? Code { get; set; }
    public string? FullName { get; set; }
    public ICollection<UserRefreshToken> UserRefreshTokens { get; set; }

    public AppUser()
    {
        UserRefreshTokens = new HashSet<UserRefreshToken>();
    }
}

