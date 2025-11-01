
namespace API.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add core services
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // Database and Cache configuration
        services.AddDatabaseConfiguration(configuration);

        // Dependency Injections
        services
            .AddInfrastructureDependencies()
            .AddServiceDependencies(configuration)
            .AddCoreDependencies()
            .AddServiceRegistration(configuration);

        // Additional services
        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        services.AddSignalR();
        services.AddScoped<INotificationSender, NotificationSender>();
        services.AddTransient<IUrlHelper>(sp =>
        {
            var actionContextAccessor = sp.GetRequiredService<IActionContextAccessor>();
            var actionContext = actionContextAccessor.ActionContext;
            
            if (actionContext == null)
            {
                throw new InvalidOperationException("ActionContext is not available. IUrlHelper can only be used within a request context.");
            }
            
            var factory = sp.GetRequiredService<IUrlHelperFactory>();
            return factory.GetUrlHelper(actionContext);
        });

        services.AddTransient<AuthFilter>();

        // Encryption
        var encryptionKey = configuration["Encryption:Key"];
        services.AddSingleton<IEncryptionProvider>(new GenerateEncryptionProvider(encryptionKey));

        return services;
    }
}

