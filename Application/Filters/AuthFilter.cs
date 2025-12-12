using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Application.Filters;

public class AuthFilter(ICurrentUserService currentUserService) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.HttpContext?.User?.Identity?.IsAuthenticated == true)
        {
            var roles = await currentUserService.GetCurrentUserRolesAsync();
            if (roles.All(x => x != "Customer"))
            {
                context.Result = new ObjectResult("Forbidden")
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }
            else
            {
                await next();
            }
        }
    }
}
