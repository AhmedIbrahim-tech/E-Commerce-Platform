using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Any;

namespace Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddServiceRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<User, Role>(option =>
        {
            // Password settings.
            option.Password.RequireDigit = true;
            option.Password.RequireLowercase = true;
            option.Password.RequireNonAlphanumeric = true;
            option.Password.RequireUppercase = true;
            option.Password.RequiredLength = 6;
            option.Password.RequiredUniqueChars = 1;

            // Lockout settings.
            option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            option.Lockout.MaxFailedAccessAttempts = 5;
            option.Lockout.AllowedForNewUsers = true;

            // User settings.
            option.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            option.User.RequireUniqueEmail = true;
            option.SignIn.RequireConfirmedEmail = true;

        }).AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

        // JWT Authentication, Email, and Paymob Payment's GateWay Settings
        services.Configure<JwtSettings>(configuration.GetSection("jwtSettings"));
        services.Configure<EmailSettings>(configuration.GetSection("emailSettings"));
        services.Configure<IdentityTablesSettings>(configuration.GetSection("IdentityTables"));
        services.Configure<DatabaseTablesSettings>(configuration.GetSection("DatabaseTables"));
        
        var paymobSettings = new PaymobSettings();
        var googleAuthSettings = new GoogleAuthSettings();
        configuration.GetSection("Paymob").Bind(paymobSettings);
        configuration.GetSection("Authorization:Google").Bind(googleAuthSettings);
        services.AddSingleton(paymobSettings);
        services.AddSingleton(googleAuthSettings);

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
       .AddJwtBearer();

        services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            var jwtSettings = services.BuildServiceProvider().GetRequiredService<IOptions<JwtSettings>>().Value;
            
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = jwtSettings.ValidateIssuer,
                ValidIssuers = new[] { jwtSettings.Issuer },
                ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret!)),
                ValidAudience = jwtSettings.Audience,
                ValidateAudience = jwtSettings.ValidateAudience,
                ValidateLifetime = jwtSettings.ValidateLifeTime,
            };
        });

        //Swagger Gn
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "E-Commerce Project",
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

        // Authorization policies
        services.AddAuthorization(option =>
        {
            option.AddPolicy("EditCustomer", policy =>
            {
                policy.RequireClaim("Edit Customer", "True");
            });
            option.AddPolicy("GetCustomer", policy =>
            {
                policy.RequireClaim("Get Customer", "True");
            });
            option.AddPolicy("GetAllCustomer", policy =>
            {
                policy.RequireClaim("Get All Customer", "True");
            });
            option.AddPolicy("DeleteCustomer", policy =>
            {
                policy.RequireClaim("Delete Customer", "True");
            });

            option.AddPolicy("CreateAdmin", policy =>
            {
                policy.RequireClaim("Create Admin", "True");
            });
            option.AddPolicy("EditAdmin", policy =>
            {
                policy.RequireClaim("Edit Admin", "True");
            });
            option.AddPolicy("GetAdmin", policy =>
            {
                policy.RequireClaim("Get Admin", "True");
            });
            option.AddPolicy("GetAllAdmin", policy =>
            {
                policy.RequireClaim("Get All Admin", "True");
            });
            option.AddPolicy("DeleteAdmin", policy =>
            {
                policy.RequireClaim("Delete Admin", "True");
            });

            option.AddPolicy("CreateEmployee", policy =>
            {
                policy.RequireClaim("Create Employee", "True");
            });
            option.AddPolicy("EditEmployee", policy =>
            {
                policy.RequireClaim("Edit Employee", "True");
            });
            option.AddPolicy("GetEmployee", policy =>
            {
                policy.RequireClaim("Get Employee", "True");
            });
            option.AddPolicy("GetAllEmployee", policy =>
            {
                policy.RequireClaim("Get All Employee", "True");
            });
            option.AddPolicy("DeleteEmployee", policy =>
            {
                policy.RequireClaim("Delete Employee", "True");
            });
        });

        return services;
    }
}
