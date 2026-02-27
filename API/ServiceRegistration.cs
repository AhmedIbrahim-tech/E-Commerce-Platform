using Application.Common.Settings;
using Hangfire;
using Hangfire.SqlServer;
using Infrastructure.Data.Authorization;
using Infrastructure.Data.Identity;
using Infrastructure.Seeder;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

namespace API;

public static class ServiceRegistration
{
    public const string CORS_POLICY_NAME = "_cors";

    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
            });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddDatabaseConfiguration(configuration);

        services
            .AddInfrastructureDependencies()
            .AddApplicationDependencies(configuration)
            .AddServiceRegistration(configuration);

        services.AddSignalR();
        services.AddScoped<INotificationSender, NotificationSender>();
        services.AddScoped<IUrlHelper>(CreateUrlHelper);
        services.AddTransient<AuthFilter>();

        var encryptionKey = configuration["Encryption:Key"];
        services.AddSingleton<IEncryptionProvider>(new GenerateEncryptionProvider(encryptionKey));

        return services;
    }

    public static IServiceCollection AddServiceRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthConfiguration(configuration);
        services.AddSwaggerGeneration(configuration);
        services.AddAuthorizationPolicies();

        return services;
    }

    public static IServiceCollection AddApplicationCors(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddCors(options =>
        {
            options.AddPolicy(CORS_POLICY_NAME, policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        return services;
    }

    public static IApplicationBuilder UseApplicationCors(this IApplicationBuilder app)
    {
        app.UseCors(CORS_POLICY_NAME);
        return app;
    }

    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var sqlConnectionString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<ApplicationDbContext>(options => 
            options.UseSqlServer(sqlConnectionString));

        services.AddMemoryCache();

        // Configure Hangfire
        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(sqlConnectionString, new Hangfire.SqlServer.SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));

        services.AddHangfireServer();

        return services;
    }

    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();
    }

    public static async Task SeedApplicationDataAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
        var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
        var defaultClaimsService = serviceProvider.GetRequiredService<IDefaultClaimsService>();

        await RoleSeeder.SeedAsync(roleManager);
        await UserSeeder.SeedAsync(userManager, dbContext, defaultClaimsService);
        
        var defaultUser = await userManager.FindByNameAsync(UserSeeder.DefaultUsername) ?? await userManager.FindByEmailAsync(UserSeeder.DefaultEmail);
        var defaultUserId = defaultUser?.Id ?? Guid.Empty;
        
        await CategorySeeder.SeedAsync(dbContext);
        await SubCategorySeeder.SeedAsync(dbContext, defaultUserId);
        await BrandSeeder.SeedAsync(dbContext, defaultUserId);
        await UnitOfMeasureSeeder.SeedAsync(dbContext, defaultUserId);
        await WarrantySeeder.SeedAsync(dbContext, defaultUserId);
        await VariantAttributeSeeder.SeedAsync(dbContext, defaultUserId);
        await TagSeeder.SeedAsync(dbContext, defaultUserId);
        await ProductSeeder.SeedAsync(dbContext, defaultUserId);
    }

    private static IUrlHelper CreateUrlHelper(IServiceProvider serviceProvider)
    {
        var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
        var httpContext = httpContextAccessor.HttpContext;
        
        if (httpContext == null)
        {
            throw new InvalidOperationException("HttpContext is not available. IUrlHelper can only be used within a request context.");
        }
        
        var actionContext = new ActionContext
        {
            HttpContext = httpContext,
            RouteData = httpContext.GetRouteData() ?? new RouteData(),
            ActionDescriptor = httpContext.GetEndpoint()?.Metadata.GetMetadata<ActionDescriptor>() ?? new ActionDescriptor()
        };
        
        var factory = serviceProvider.GetRequiredService<IUrlHelperFactory>();
        return factory.GetUrlHelper(actionContext);
    }

    private static IServiceCollection AddAuthConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<AppUser, AppRole>(ConfigureIdentityOptions)
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddApplicationSettings(configuration);
        services.AddAuthentication(ConfigureAuthenticationOptions)
            .AddJwtBearer();

        services.AddSingleton<IConfigureOptions<JwtBearerOptions>>(serviceProvider =>
            new ConfigureNamedOptions<JwtBearerOptions>(
                JwtBearerDefaults.AuthenticationScheme,
                options => ConfigureJwtBearerOptions(options, serviceProvider)));

        return services;
    }

    private static void ConfigureIdentityOptions(IdentityOptions options)
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 8;
        options.Password.RequiredUniqueChars = 1;

        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;

        options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedEmail = true;
    }

    private static void ConfigureAuthenticationOptions(AuthenticationOptions options)
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }

    private static IServiceCollection AddApplicationSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("jwtSettings"));
        services.Configure<EmailSettings>(configuration.GetSection("emailSettings"));
        services.Configure<DatabaseTablesSettings>(configuration.GetSection("DatabaseTables"));
        
        services.Configure<PaymobSettings>(configuration.GetSection("Paymob"));
        services.AddSingleton(serviceProvider => 
            serviceProvider.GetRequiredService<IOptions<PaymobSettings>>().Value);
        
        services.Configure<GoogleAuthSettings>(configuration.GetSection("Authorization:Google"));
        services.AddSingleton(serviceProvider => 
            serviceProvider.GetRequiredService<IOptions<GoogleAuthSettings>>().Value);

        return services;
    }

    private static void ConfigureJwtBearerOptions(JwtBearerOptions options, IServiceProvider serviceProvider)
    {
        var jwtSettings = serviceProvider.GetRequiredService<IOptions<JwtSettings>>().Value;

        if (string.IsNullOrWhiteSpace(jwtSettings.Secret))
        {
            throw new InvalidOperationException("JWT Secret is not configured. Please set jwtSettings:Secret in appsettings.json");
        }

        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = CreateTokenValidationParameters(jwtSettings);
        options.Events = CreateJwtBearerEvents();
    }

    private static TokenValidationParameters CreateTokenValidationParameters(JwtSettings jwtSettings)
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = jwtSettings.ValidateIssuer,
            ValidIssuers = !string.IsNullOrWhiteSpace(jwtSettings.Issuer)
                ? new[] { jwtSettings.Issuer }
                : null,
            ValidateAudience = jwtSettings.ValidateAudience,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = jwtSettings.ValidateLifeTime,
            ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
            ClockSkew = TimeSpan.Zero,
            RequireExpirationTime = true,
            RequireSignedTokens = true,
        };
    }

    private static JwtBearerEvents CreateJwtBearerEvents()
    {
        return new JwtBearerEvents
        {
            OnAuthenticationFailed = _ => Task.CompletedTask,
            OnTokenValidated = _ => Task.CompletedTask,
            OnChallenge = _ => Task.CompletedTask
        };
    }

    private static IServiceCollection AddSwaggerGeneration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Tajerly Project",
                Version = "v1",
                Description = "Clean Architecture Project"
            });

            options.EnableAnnotations();
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, CreateJwtSecurityScheme());
        });

        return services;
    }

    private static OpenApiSecurityScheme CreateJwtSecurityScheme()
    {
        return new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer 12345abcdef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            BearerFormat = "JWT"
        };
    }

    private static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        var authorizationBuilder = services.AddAuthorizationBuilder();
        var permissions = Permissions.GetAll();

        foreach (var permission in permissions)
        {
            var policyName = permission.Replace(".", string.Empty);
            authorizationBuilder.AddPolicy(policyName, policy => 
                policy.RequireClaim(permission, "True"));
        }

        return services;
    }
}

