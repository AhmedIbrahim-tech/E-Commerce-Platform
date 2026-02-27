using Application.Common.Bases;
using Application.ServicesHandlers.Auth;
using Application.ServicesHandlers.Services;
using Application.Wrappers;
using Infrastructure.Data.Identity;
using Infrastructure.RepositoriesHandlers.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ApplicationUser.Queries.GetUsersPaginatedList;

public class GetUsersPaginatedListQueryHandler(
    IUnitOfWork unitOfWork,
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

        var baseQuery = from user in unitOfWork.Context.Users.AsNoTracking()
                        join userRole in unitOfWork.Context.Set<IdentityUserRole<Guid>>() on user.Id equals userRole.UserId into userRoles
                        from ur in userRoles.DefaultIfEmpty()
                        join role in unitOfWork.Context.Set<AppRole>() on ur.RoleId equals role.Id into roles
                        from r in roles.DefaultIfEmpty()
                        select new { User = user, RoleName = r != null ? r.Name : null };

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();
            baseQuery = baseQuery.Where(x =>
                (x.User.DisplayName != null && x.User.DisplayName.Contains(search)) ||
                (x.User.Email != null && x.User.Email.Contains(search)) ||
                (x.User.UserName != null && x.User.UserName.Contains(search)));
        }

        if (request.IsActive.HasValue)
        {
            if (request.IsActive.Value)
            {
                baseQuery = baseQuery.Where(x => x.User.LockoutEnd == null || x.User.LockoutEnd <= DateTimeOffset.UtcNow);
            }
            else
            {
                baseQuery = baseQuery.Where(x => x.User.LockoutEnd != null && x.User.LockoutEnd > DateTimeOffset.UtcNow);
            }
        }

        if (!string.IsNullOrWhiteSpace(request.Role) && request.Role != "All")
        {
            baseQuery = baseQuery.Where(x => x.RoleName == request.Role);
        }

        var userQuery = baseQuery.Select(x => x.User).Distinct();

        IQueryable<AppUser> sortedQuery = request.SortBy switch
        {
            1 => userQuery.OrderBy(u => u.DisplayName ?? u.UserName ?? ""),
            2 => userQuery.OrderByDescending(u => u.DisplayName ?? u.UserName ?? ""),
            3 => userQuery.OrderBy(u => u.Email ?? ""),
            4 => userQuery.OrderByDescending(u => u.Email ?? ""),
            5 => userQuery.OrderBy(u => u.Id),
            6 => userQuery.OrderByDescending(u => u.Id),
            _ => userQuery.OrderByDescending(u => u.Id),
        };

        var totalCount = await sortedQuery.CountAsync(cancellationToken);

        var skip = (request.PageNumber - 1) * request.PageSize;
        var userIds = await sortedQuery
            .Skip(skip)
            .Take(request.PageSize)
            .Select(u => u.Id)
            .ToListAsync(cancellationToken);

        if (userIds.Count == 0)
        {
            return PaginatedResult<GetUsersPaginatedListResponse>.Success(new List<GetUsersPaginatedListResponse>(), totalCount, request.PageNumber, request.PageSize);
        }

        var users = await unitOfWork.Context.Users
            .AsNoTracking()
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync(cancellationToken);

        var userRoleMap = await (from ur in unitOfWork.Context.Set<IdentityUserRole<Guid>>()
                                  join r in unitOfWork.Context.Set<AppRole>() on ur.RoleId equals r.Id
                                  where userIds.Contains(ur.UserId)
                                  select new { ur.UserId, r.Name })
                                  .GroupBy(x => x.UserId)
                                  .ToDictionaryAsync(g => g.Key, g => g.First().Name, cancellationToken);

        var adminMap = await unitOfWork.Admins.GetTableNoTracking()
            .Where(a => userIds.Contains(a.AppUserId))
            .ToDictionaryAsync(a => a.AppUserId, a => new { a.Id, a.FullName }, cancellationToken);

        var vendorMap = await unitOfWork.Vendors.GetTableNoTracking()
            .Where(v => userIds.Contains(v.AppUserId))
            .ToDictionaryAsync(v => v.AppUserId, v => new { v.Id, v.FullName }, cancellationToken);

        var customerMap = await unitOfWork.Customers.GetTableNoTracking()
            .Where(c => userIds.Contains(c.AppUserId))
            .ToDictionaryAsync(c => c.AppUserId, c => new { c.Id, c.FullName }, cancellationToken);

        var responses = users.Select(user =>
        {
            var role = userRoleMap.GetValueOrDefault(user.Id) ?? "Customer";
            string? fullName = user.DisplayName;
            Guid entityId = user.Id;

            if (adminMap.TryGetValue(user.Id, out var admin))
            {
                fullName = admin.FullName ?? fullName;
                entityId = admin.Id;
            }
            else if (vendorMap.TryGetValue(user.Id, out var vendor))
            {
                fullName = vendor.FullName ?? fullName;
                entityId = vendor.Id;
            }
            else if (customerMap.TryGetValue(user.Id, out var customer))
            {
                fullName = customer.FullName ?? fullName;
                entityId = customer.Id;
            }

            var isActive = user.LockoutEnd == null || user.LockoutEnd <= DateTimeOffset.UtcNow;

            return new GetUsersPaginatedListResponse
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
                CreatedAt = null
            };
        }).ToList();

        return PaginatedResult<GetUsersPaginatedListResponse>.Success(responses, totalCount, request.PageNumber, request.PageSize);
    }
}
