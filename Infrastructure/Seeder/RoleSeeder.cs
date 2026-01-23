using Infrastructure.Data.Authorization;

namespace Infrastructure.Seeder;

public static class RoleSeeder
{
    public static async Task SeedAsync(RoleManager<AppRole> roleManager)
    {
        var rolesCount = await roleManager.Roles.CountAsync();
        if (rolesCount <= 0)
        {
            await roleManager.CreateAsync(new AppRole() { Name = Roles.SuperAdmin });
            await roleManager.CreateAsync(new AppRole() { Name = Roles.Admin });
            await roleManager.CreateAsync(new AppRole() { Name = Roles.Customer });
            await roleManager.CreateAsync(new AppRole() { Name = Roles.Merchant });
        }
    }
}
