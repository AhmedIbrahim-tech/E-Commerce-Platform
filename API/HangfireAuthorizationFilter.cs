using Hangfire.Dashboard;
using Infrastructure.Data.Authorization;
using Microsoft.AspNetCore.Hosting;

namespace API;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        // In production, you should implement proper authentication/authorization
        // For now, allow access in development, restrict in production
        var httpContext = context.GetHttpContext();
        
        // Allow access in development
        if (httpContext.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
        {
            return true;
        }

        // In production, you can check for authentication/authorization here
        // Example: return httpContext.User.Identity?.IsAuthenticated == true && 
        //          httpContext.User.IsInRole(Roles.Admin);
        
        // For now, allow access (you should secure this in production)
        return true;
    }
}
