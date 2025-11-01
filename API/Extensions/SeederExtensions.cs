
namespace API.Extensions;

public static class SeederExtensions
{
    public static async Task SeedApplicationDataAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

        await RoleSeeder.SeedAsync(roleManager);
        await UserSeeder.SeedAsync(userManager);
    }
}

