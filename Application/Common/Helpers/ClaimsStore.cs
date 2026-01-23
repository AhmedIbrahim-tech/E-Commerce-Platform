using Infrastructure.Data.Authorization;

namespace Application.Common.Helpers;

public static class ClaimsStore
{
    public static List<Claim> GetAllClaims()
    {
        return Permissions.GetAll()
            .Select(permission => new Claim(permission, "False"))
            .ToList();
    }
}
