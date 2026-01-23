using System.Security.Claims;
using Infrastructure.Seeder;

namespace Infrastructure.Data.Authorization;

public interface IDefaultClaimsService
{
    Task<IdentityResult> AssignDefaultClaimsAsync(AppUser user, string roleName);
}

public class DefaultClaimsService(UserManager<AppUser> userManager) : IDefaultClaimsService
{
    public async Task<IdentityResult> AssignDefaultClaimsAsync(AppUser user, string roleName)
    {
        var defaultClaims = Permissions.GetDefaultForRole(roleName);
        
        if (defaultClaims.Count == 0)
        {
            await UserSeeder.SyncAllClaimsToDefaultUserAsync(userManager);
            return IdentityResult.Success;
        }

        var claims = defaultClaims.Select(claim => new Claim(claim, "True")).ToList();
        var result = await userManager.AddClaimsAsync(user, claims);
        
        await UserSeeder.SyncAllClaimsToDefaultUserAsync(userManager);
        
        return result;
    }
}
