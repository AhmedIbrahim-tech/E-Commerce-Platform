Env.Load(); // Load environment variables from .env file

var builder = WebApplication.CreateBuilder(args);

// Add application services
builder.Services.AddApplicationServices(builder.Configuration);

// Configure CORS
builder.Services.AddApplicationCors();

var app = builder.Build();

// Apply database migrations automatically
await app.ApplyMigrationsAsync();

// Seed application data
await app.SeedApplicationDataAsync();

// Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseHttpsRedirection();
app.UseApplicationCors();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationsHub>("/hubs/notifications");

app.Run();
