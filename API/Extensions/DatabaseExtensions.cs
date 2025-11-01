
namespace API.Extensions;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        // SQL Server connection
        var sqlConnectionString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<ApplicationContext>(options => 
            options.UseSqlServer(sqlConnectionString));

        // Memory Cache configuration
        services.AddMemoryCache();

        return services;
    }
}

