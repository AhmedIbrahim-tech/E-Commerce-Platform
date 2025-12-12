using Application.Common.Behaviors;
using Application.ServicesHandlers.AzureTranslation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;

public static class ModuleApplicationDependencies
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        /// Configuaration for MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        /// Configuaration for FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddSingleton<AzureTranslationService>();

        /// Third Party Services Registration
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IAuthenticationService, AuthenticationService>();
        services.AddTransient<IAuthGoogleService, AuthGoogleService>();
        services.AddTransient<ICurrentUserService, CurrentUserService>();
        services.AddTransient<IFileService, FileService>();

        services.AddPaymobCashIn(options =>
        {
            options.ApiKey = configuration["Paymob:ApiKey"];
            options.Hmac = configuration["Paymob:HMAC"];
        });

        return services;
    }
}
