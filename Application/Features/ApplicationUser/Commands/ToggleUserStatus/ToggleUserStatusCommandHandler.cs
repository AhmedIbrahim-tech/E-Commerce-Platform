using Application.Common.Bases;
using Application.Common.Errors;
using Application.ServicesHandlers.Auth;
using Infrastructure.Data;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ApplicationUser.Commands.ToggleUserStatus;

public class ToggleUserStatusCommandHandler(
    ApplicationDbContext dbContext,
    UserManager<AppUser> userManager,
    ICurrentUserService currentUserService) : ApiResponseHandler(),
    IRequestHandler<ToggleUserStatusCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(ToggleUserStatusCommand request, CancellationToken cancellationToken)
    {
        // Find AppUser by entity ID
        AppUser? appUser = null;

        var admin = await dbContext.Admins
            .AsTracking()
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
        if (admin != null)
        {
            appUser = await userManager.FindByIdAsync(admin.AppUserId.ToString());
        }
        else
        {
            var vendor = await dbContext.Vendors
                .AsTracking()
                .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);
            if (vendor != null)
            {
                appUser = await userManager.FindByIdAsync(vendor.AppUserId.ToString());
            }
            else
            {
                var customer = await dbContext.Customers
                    .AsTracking()
                    .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
                if (customer != null)
                {
                    appUser = await userManager.FindByIdAsync(customer.AppUserId.ToString());
                }
                else
                {
                    // Try as AppUserId directly
                    appUser = await userManager.FindByIdAsync(request.Id.ToString());
                }
            }
        }

        if (appUser == null)
            return NotFound<string>("User not found");

        // Toggle lockout (inactive = locked)
        if (request.IsActive)
        {
            // Unlock user
            appUser.LockoutEnd = null;
        }
        else
        {
            // Lock user (set lockout to far future)
            appUser.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100);
        }

        var result = await userManager.UpdateAsync(appUser);
        if (!result.Succeeded)
            return BadRequest<string>("Failed to update user status");

        return Success("User status updated successfully");
    }
}
