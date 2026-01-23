using Application.Common.Bases;
using Application.ServicesHandlers.Auth;
using Application.ServicesHandlers.Services;
using Infrastructure.Data;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ApplicationUser.Queries.GetUserById;

public class GetUserByIdQueryHandler(
    ApplicationDbContext dbContext,
    UserManager<AppUser> userManager,
    ICurrentUserService currentUserService,
    IFileUploadService fileUploadService) : ApiResponseHandler(),
    IRequestHandler<GetUserByIdQuery, ApiResponse<GetUserByIdResponse>>
{
    public async Task<ApiResponse<GetUserByIdResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        // Try to find user by entity ID (Admin/Vendor/Customer) or AppUserId
        AppUser? appUser = null;
        Guid entityId = request.Id;
        string? fullName = null;

        // Check Admin
        var admin = await dbContext.Admins
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
        if (admin != null)
        {
            entityId = admin.Id;
            appUser = await userManager.FindByIdAsync(admin.AppUserId.ToString());
            fullName = admin.FullName;
        }
        else
        {
            // Check Vendor
            var vendor = await dbContext.Vendors
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);
            if (vendor != null)
            {
                entityId = vendor.Id;
                appUser = await userManager.FindByIdAsync(vendor.AppUserId.ToString());
                fullName = vendor.FullName;
            }
            else
            {
                // Check Customer
                var customer = await dbContext.Customers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
                if (customer != null)
                {
                    entityId = customer.Id;
                    appUser = await userManager.FindByIdAsync(customer.AppUserId.ToString());
                    fullName = customer.FullName;
                }
                else
                {
                    // Try as AppUserId directly
                    appUser = await userManager.FindByIdAsync(request.Id.ToString());
                    if (appUser != null)
                    {
                        entityId = appUser.Id;
                        fullName = appUser.DisplayName;
                    }
                }
            }
        }

        if (appUser == null)
            return NotFound<GetUserByIdResponse>("User not found");

        var roles = await userManager.GetRolesAsync(appUser);
        var role = roles.FirstOrDefault() ?? "Customer";

        var claims = await userManager.GetClaimsAsync(appUser);
        var claimValues = claims.Select(c => c.Value).ToList();

        var isActive = appUser.LockoutEnd == null || appUser.LockoutEnd <= DateTimeOffset.UtcNow;

        // Get last login from refresh tokens (simplified - you may want to track this separately)
        var lastLogin = await dbContext.UserRefreshTokens
            .Where(t => t.AppUserId == appUser.Id)
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => t.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        var response = new GetUserByIdResponse
        {
            Id = entityId,
            AppUserId = appUser.Id,
            FullName = fullName ?? appUser.DisplayName,
            UserName = appUser.UserName,
            Email = appUser.Email,
            PhoneNumber = appUser.PhoneNumber,
            Role = role,
            Roles = roles.ToList(),
            ProfileImageUrl = fileUploadService.ToAbsoluteUrl(appUser.ProfileImage),
            IsActive = isActive,
            CreatedAt = null, // AppUser doesn't track CreatedTime - can be enhanced later
            LastLoginAt = lastLogin != default ? lastLogin : null,
            Claims = claimValues
        };

        return Success(response);
    }
}
