using Application.Common.Settings;
using Application.Filters;
using Application.ServicesHandlers.Services;
using Infrastructure.Data;
using Infrastructure.Data.Identity;
using Infrastructure.Seeder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.Text;

namespace API.Extensions;

public static class ServiceRegistration
{
    public const string CORS_POLICY_NAME = "_cors";

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
            .AddApplicationDependencies(configuration)
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

    public static IServiceCollection AddServiceRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthConfiguration(configuration);
        services.AddSwaggerGeneration(configuration);
        services.AddAuthorizationPolicies();

        return services;
    }

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

    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var sqlConnectionString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<ApplicationDbContext>(options => 
            options.UseSqlServer(sqlConnectionString));

        services.AddMemoryCache();

        return services;
    }

    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            await context.Database.MigrateAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public static async Task SeedApplicationDataAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await RoleSeeder.SeedAsync(roleManager);
        await UserSeeder.SeedAsync(userManager, dbContext);
    }

    private static IServiceCollection AddAuthConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<AppUser, AppRole>(options =>
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

            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        services.Configure<JwtSettings>(configuration.GetSection("jwtSettings"));
        services.Configure<EmailSettings>(configuration.GetSection("emailSettings"));
        services.Configure<DatabaseTablesSettings>(configuration.GetSection("DatabaseTables"));
        
        services.Configure<PaymobSettings>(configuration.GetSection("Paymob"));
        services.AddSingleton(serviceProvider => 
            serviceProvider.GetRequiredService<IOptions<PaymobSettings>>().Value);
        
        services.Configure<GoogleAuthSettings>(configuration.GetSection("Authorization:Google"));
        services.AddSingleton(serviceProvider => 
            serviceProvider.GetRequiredService<IOptions<GoogleAuthSettings>>().Value);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer();

        services.AddSingleton<IConfigureOptions<JwtBearerOptions>>(serviceProvider =>
        {
            return new ConfigureNamedOptions<JwtBearerOptions>(
                JwtBearerDefaults.AuthenticationScheme,
                options =>
                {
                    var jwtSettings = serviceProvider.GetRequiredService<IOptions<JwtSettings>>().Value;

                    if (string.IsNullOrWhiteSpace(jwtSettings.Secret))
                    {
                        throw new InvalidOperationException("JWT Secret is not configured. Please set jwtSettings:Secret in appsettings.json");
                    }

                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;

                    options.TokenValidationParameters = new TokenValidationParameters
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

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            return Task.CompletedTask;
                        }
                    };
                });
        });

        return services;
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

            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    Array.Empty<string>()
                }
            });

            options.MapType<TimeOnly>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "time",
                Example = new OpenApiString("14:30:00")
            });

            options.MapType<TimeOnly?>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "time",
                Nullable = true,
                Example = new OpenApiString("14:30:00")
            });
        });

        return services;
    }

    private static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        var claimTypes = new[]
        {
            "Edit Customer",
            "Get Customer",
            "Get All Customer",
            "Delete Customer",
            "Create Admin",
            "Edit Admin",
            "Get Admin",
            "Get All Admin",
            "Delete Admin",
            "Create Employee",
            "Edit Employee",
            "Get Employee",
            "Get All Employee",
            "Delete Employee"
        };

        var authorizationBuilder = services.AddAuthorizationBuilder();

        foreach (var claimType in claimTypes)
        {
            var policyName = claimType.Replace(" ", string.Empty);

            authorizationBuilder.AddPolicy(policyName, policy =>
            {
                policy.RequireClaim(claimType, "True");
            });
        }

        return services;
    }
}

