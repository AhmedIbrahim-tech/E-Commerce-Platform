using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class DatabaseMigrationExtensions
{
    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<ApplicationContext>();
            await context.Database.MigrateAsync();
        }
        catch (Exception)
        {
            throw; // مهم علشان التطبيق يعرف لو في مشكلة
        }
    }

}