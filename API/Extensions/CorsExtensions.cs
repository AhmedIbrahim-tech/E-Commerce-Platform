
namespace API.Extensions;

public static class CorsExtensions
{
    public const string CORS_POLICY_NAME = "_cors";

    public static IServiceCollection AddApplicationCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(CORS_POLICY_NAME, policy =>
            {
                policy.AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowAnyOrigin();
            });
        });

        return services;
    }

    public static IApplicationBuilder UseApplicationCors(this IApplicationBuilder app)
    {
        app.UseCors(CORS_POLICY_NAME);
        return app;
    }
}

