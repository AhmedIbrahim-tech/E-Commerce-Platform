using Hangfire.Dashboard;
using Infrastructure.Data.Authorization;
using Microsoft.AspNetCore.Hosting;

namespace API;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        var env = httpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();

        if (env.IsDevelopment())
        {
            return true;
        }

        var user = httpContext.User;
        if (user.Identity is not { IsAuthenticated: true })
        {
            return false;
        }

        return user.IsInRole(Roles.SuperAdmin) || user.IsInRole(Roles.Admin);
    }
}
