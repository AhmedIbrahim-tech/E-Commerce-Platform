using Application.Common.Bases;
using Application.ServicesHandlers.Services;
using Application.Wrappers;
using Domain.Entities.Users;
using Infrastructure.Data.Identity;
using Infrastructure.RepositoriesHandlers.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Admins.Queries.GetAdminPaginatedList;

public class GetAdminPaginatedListQueryHandler(
    IUnitOfWork unitOfWork,
    UserManager<AppUser> userManager,
    IFileUploadService fileUploadService) : ApiResponseHandler(),
    IRequestHandler<GetAdminPaginatedListQuery, PaginatedResult<GetAdminPaginatedListResponse>>
{
    public async Task<PaginatedResult<GetAdminPaginatedListResponse>> Handle(GetAdminPaginatedListQuery request, CancellationToken cancellationToken)
    {
        var adminsQuery = unitOfWork.Admins.GetTableNoTracking()
            .IgnoreQueryFilters()
            .ApplyFiltering(request.SortBy, request.Search);

        var adminsWithAppUsers = await adminsQuery
            .Join(userManager.Users.AsNoTracking(),
                admin => admin.AppUserId,
                appUser => appUser.Id,
                (admin, appUser) => new
                {
                    Admin = admin,
                    AppUser = appUser
                })
            .ToPaginatedListAsync(request.PageNumber, request.PageSize);

        if (!adminsWithAppUsers.Data.Any())
        {
            return new PaginatedResult<GetAdminPaginatedListResponse>(new List<GetAdminPaginatedListResponse>())
            {
                CurrentPage = request.PageNumber,
                TotalPages = 0,
                TotalCount = 0,
                PageSize = request.PageSize,
                Succeeded = true
            };
        }

        var userIds = adminsWithAppUsers.Data.Select(x => x.AppUser.Id).ToList();
        
        var userRoleIds = await (from ur in unitOfWork.Context.Set<Microsoft.AspNetCore.Identity.IdentityUserRole<Guid>>()
                                  where userIds.Contains(ur.UserId)
                                  select new { ur.UserId, ur.RoleId })
                                  .ToListAsync(cancellationToken);

        var roleIds = userRoleIds.Select(x => x.RoleId).Distinct().ToList();
        var rolesDict = await unitOfWork.Context.Set<AppRole>()
            .AsNoTracking()
            .Where(r => roleIds.Contains(r.Id))
            .ToDictionaryAsync(r => r.Id, r => r.Name ?? string.Empty, cancellationToken);

        var userRolesDict = userRoleIds
            .GroupBy(x => x.UserId)
            .ToDictionary(
                g => g.Key,
                g => rolesDict.GetValueOrDefault(g.First().RoleId, string.Empty)
            );

        var responses = adminsWithAppUsers.Data.Select(item => new GetAdminPaginatedListResponse
        {
            Id = item.Admin.Id,
            AppUserId = item.Admin.AppUserId,
            FullName = item.Admin.FullName ?? string.Empty,
            Email = item.AppUser.Email ?? string.Empty,
            Gender = item.Admin.Gender,
            PhoneNumber = item.AppUser.PhoneNumber ?? string.Empty,
            Address = item.Admin.Address ?? string.Empty,
            Role = userRolesDict.GetValueOrDefault(item.AppUser.Id, string.Empty),
            ProfileImage = fileUploadService.ToAbsoluteUrl(item.AppUser.ProfileImage),
            IsDeleted = item.Admin.IsDeleted
        }).ToList();

        var result = new PaginatedResult<GetAdminPaginatedListResponse>(responses)
        {
            CurrentPage = adminsWithAppUsers.CurrentPage,
            TotalPages = adminsWithAppUsers.TotalPages,
            TotalCount = adminsWithAppUsers.TotalCount,
            PageSize = adminsWithAppUsers.PageSize,
            Succeeded = adminsWithAppUsers.Succeeded,
            Messages = adminsWithAppUsers.Messages,
            Meta = new { Count = responses.Count }
        };
        
        return result;
    }
}
