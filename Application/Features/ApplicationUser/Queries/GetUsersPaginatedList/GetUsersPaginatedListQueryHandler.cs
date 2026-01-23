using Application.Common.Bases;
using Application.ServicesHandlers.Auth;
using Application.ServicesHandlers.Services;
using Application.Wrappers;
using Infrastructure.Data;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ApplicationUser.Queries.GetUsersPaginatedList;

public class GetUsersPaginatedListQueryHandler(
    ApplicationDbContext dbContext,
    UserManager<AppUser> userManager,
    ICurrentUserService currentUserService,
    IFileUploadService fileUploadService) : ApiResponseHandler(),
    IRequestHandler<GetUsersPaginatedListQuery, PaginatedResult<GetUsersPaginatedListResponse>>
{
    public async Task<PaginatedResult<GetUsersPaginatedListResponse>> Handle(GetUsersPaginatedListQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.GetUserId();
        var currentUser = await userManager.FindByIdAsync(currentUserId.ToString());
        if (currentUser == null)
            return PaginatedResult<GetUsersPaginatedListResponse>.Success(new List<GetUsersPaginatedListResponse>(), 0, request.PageNumber, request.PageSize);

        var currentUserRoles = await userManager.GetRolesAsync(currentUser);
        var currentUserRole = currentUserRoles.FirstOrDefault() ?? string.Empty;

        // Build base query from AppUsers
        var query = userManager.Users.AsQueryable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();
            query = query.Where(u =>
                (u.DisplayName != null && u.DisplayName.Contains(search)) ||
                (u.Email != null && u.Email.Contains(search)) ||
                (u.UserName != null && u.UserName.Contains(search)));
        }

        // Apply active/inactive filter
        if (request.IsActive.HasValue)
        {
            if (request.IsActive.Value)
            {
                // Active: not locked
                query = query.Where(u => u.LockoutEnd == null || u.LockoutEnd <= DateTimeOffset.UtcNow);
            }
            else
            {
                // Inactive: locked
                query = query.Where(u => u.LockoutEnd != null && u.LockoutEnd > DateTimeOffset.UtcNow);
            }
        }

        // Apply sorting
        IQueryable<AppUser> sortedQuery = request.SortBy switch
        {
            1 => query.OrderBy(u => u.DisplayName ?? u.UserName ?? ""), // NameAsc
            2 => query.OrderByDescending(u => u.DisplayName ?? u.UserName ?? ""), // NameDesc
            3 => query.OrderBy(u => u.Email ?? ""), // EmailAsc
            4 => query.OrderByDescending(u => u.Email ?? ""), // EmailDesc
            5 => query.OrderBy(u => u.Id), // CreatedAtAsc (using Id as proxy)
            6 => query.OrderByDescending(u => u.Id), // CreatedAtDesc (using Id as proxy)
            _ => query.OrderByDescending(u => u.Id), // Default: CreatedAtDesc (using Id as proxy)
        };

        // Get all users first (we need to check roles which requires async calls)
        var allUsers = await sortedQuery.ToListAsync(cancellationToken);

        // Filter by role if specified (must be done after fetching users)
        var filteredUsers = allUsers;
        if (!string.IsNullOrWhiteSpace(request.Role) && request.Role != "All")
        {
            var roleFilter = request.Role;
            var usersWithRole = new List<AppUser>();
            foreach (var user in allUsers)
            {
                var roles = await userManager.GetRolesAsync(user);
                if (roles.Contains(roleFilter))
                    usersWithRole.Add(user);
            }
            filteredUsers = usersWithRole;
        }

        // Get total count after role filtering
        var totalCount = filteredUsers.Count;

        // Apply pagination
        var skip = (request.PageNumber - 1) * request.PageSize;
        var users = filteredUsers.Skip(skip).Take(request.PageSize).ToList();

        // Build response with role information
        var responses = new List<GetUsersPaginatedListResponse>();
        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "Customer";

            // Get full name from related entity (Admin/Vendor/Customer)
            string? fullName = user.DisplayName;
            Guid entityId = user.Id;

            // Try to get full name from Admin
            var admin = await dbContext.Admins
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.AppUserId == user.Id, cancellationToken);
            if (admin != null)
            {
                fullName = admin.FullName ?? fullName;
                entityId = admin.Id;
            }
            else
            {
                // Try Vendor
                var vendor = await dbContext.Vendors
                    .AsNoTracking()
                    .FirstOrDefaultAsync(v => v.AppUserId == user.Id, cancellationToken);
                if (vendor != null)
                {
                    fullName = vendor.FullName ?? fullName;
                    entityId = vendor.Id;
                }
                else
                {
                    // Try Customer
                    var customer = await dbContext.Customers
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => c.AppUserId == user.Id, cancellationToken);
                    if (customer != null)
                    {
                        fullName = customer.FullName ?? fullName;
                        entityId = customer.Id;
                    }
                }
            }

            var isActive = user.LockoutEnd == null || user.LockoutEnd <= DateTimeOffset.UtcNow;

            responses.Add(new GetUsersPaginatedListResponse
            {
                Id = entityId,
                AppUserId = user.Id,
                FullName = fullName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = role,
                ProfileImageUrl = fileUploadService.ToAbsoluteUrl(user.ProfileImage),
                IsActive = isActive,
                CreatedAt = null // AppUser doesn't track CreatedTime - can be enhanced later
            });
        }

        return PaginatedResult<GetUsersPaginatedListResponse>.Success(responses, totalCount, request.PageNumber, request.PageSize);
    }
}
