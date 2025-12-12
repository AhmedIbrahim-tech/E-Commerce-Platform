using Infrastructure.Data.Identity;

namespace Infrastructure.Seeder
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(RoleManager<AppRole> roleManager)
        {
            var rolesCount = await roleManager.Roles.CountAsync();
            if (rolesCount <= 0)
            {
                await roleManager.CreateAsync(new AppRole() { Name = "Admin" });
                await roleManager.CreateAsync(new AppRole() { Name = "Employee" });
                await roleManager.CreateAsync(new AppRole() { Name = "Customer" });
            }
        }
    }
}
