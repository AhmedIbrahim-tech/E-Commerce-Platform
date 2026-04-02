using API;
using Hangfire;

Env.Load(); // Load environment variables from .env file

var builder = WebApplication.CreateBuilder(args);

// Add application services
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddApplicationCors(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.ApplyMigrationsAsync();
    await app.SeedApplicationDataAsync();
}

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);

        options.IndexStream = () =>
            File.OpenRead(Path.Combine(
                app.Environment.ContentRootPath,
                "wwwroot",
                "swagger",
                "index.html"
            ));
    });
}

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseHttpsRedirection();
app.UseApplicationCors();
app.UseAuthentication();
app.UseAuthorization();

// Hangfire Dashboard (consider adding authentication in production)
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    DashboardTitle = "Tajerly Hangfire Dashboard",
    Authorization = new[] { new HangfireAuthorizationFilter() }
});

app.MapControllers();
app.MapHub<NotificationsHub>("/hubs/notifications");

app.Run();
