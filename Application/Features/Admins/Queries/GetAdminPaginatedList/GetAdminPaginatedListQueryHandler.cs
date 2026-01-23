using Application.Common.Bases;
using Application.ServicesHandlers.Services;
using Application.Wrappers;
using Domain.Entities.Users;
using Infrastructure.Data;
using Infrastructure.Data.Identity;

namespace Application.Features.Admins.Queries.GetAdminPaginatedList;

public class GetAdminPaginatedListQueryHandler(
    ApplicationDbContext dbContext,
    UserManager<AppUser> userManager,
    IFileUploadService fileUploadService) : ApiResponseHandler(),
    IRequestHandler<GetAdminPaginatedListQuery, PaginatedResult<GetAdminPaginatedListResponse>>
{
    public async Task<PaginatedResult<GetAdminPaginatedListResponse>> Handle(GetAdminPaginatedListQuery request, CancellationToken cancellationToken)
    {
        var adminsQuery = dbContext.Admins
            .IgnoreQueryFilters()
            .AsNoTracking()
            .AsQueryable()
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

        var userIds = adminsWithAppUsers.Data.Select(x => x.AppUser.Id).ToList();
        var userRolesDict = new Dictionary<Guid, string>();
        
        foreach (var item in adminsWithAppUsers.Data)
        {
            var roles = await userManager.GetRolesAsync(item.AppUser);
            userRolesDict[item.AppUser.Id] = roles.FirstOrDefault() ?? string.Empty;
        }

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
