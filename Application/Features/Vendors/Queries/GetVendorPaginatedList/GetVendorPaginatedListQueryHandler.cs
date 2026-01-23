using Application.Common.Bases;
using Application.ServicesHandlers.Services;
using Application.Wrappers;
using Infrastructure.Data.Identity;

namespace Application.Features.Vendors.Queries.GetVendorPaginatedList;

public class GetVendorPaginatedListQueryHandler(
    ApplicationDbContext dbContext,
    UserManager<AppUser> userManager,
    IFileUploadService fileUploadService) : ApiResponseHandler(),
    IRequestHandler<GetVendorPaginatedListQuery, PaginatedResult<GetVendorPaginatedListResponse>>
{
    public async Task<PaginatedResult<GetVendorPaginatedListResponse>> Handle(GetVendorPaginatedListQuery request, CancellationToken cancellationToken)
    {
        var vendorsQuery = dbContext.Vendors
            .IgnoreQueryFilters()
            .AsNoTracking()
            .AsQueryable()
            .ApplyFiltering(request.SortBy, request.Search);

        var vendorsWithAppUsers = await vendorsQuery
            .Join(userManager.Users.AsNoTracking(),
                vendor => vendor.AppUserId,
                appUser => appUser.Id,
                (vendor, appUser) => new
                {
                    Vendor = vendor,
                    AppUser = appUser
                })
            .ToPaginatedListAsync(request.PageNumber, request.PageSize);

        var userIds = vendorsWithAppUsers.Data.Select(x => x.AppUser.Id).ToList();
        var userRolesDict = new Dictionary<Guid, string>();
        
        foreach (var item in vendorsWithAppUsers.Data)
        {
            var roles = await userManager.GetRolesAsync(item.AppUser);
            userRolesDict[item.AppUser.Id] = roles.FirstOrDefault() ?? string.Empty;
        }

        var responses = vendorsWithAppUsers.Data.Select(item => new GetVendorPaginatedListResponse
        {
            Id = item.Vendor.Id,
            FullName = item.Vendor.FullName ?? string.Empty,
            UserName = item.AppUser.UserName ?? string.Empty,
            Email = item.AppUser.Email ?? string.Empty,
            Gender = item.Vendor.Gender,
            PhoneNumber = item.AppUser.PhoneNumber ?? string.Empty,
            StoreName = item.Vendor.StoreName ?? string.Empty,
            CommissionRate = item.Vendor.CommissionRate,
            Role = userRolesDict.GetValueOrDefault(item.AppUser.Id, string.Empty),
            ProfileImage = fileUploadService.ToAbsoluteUrl(item.AppUser.ProfileImage),
            IsDeleted = item.Vendor.IsDeleted
        }).ToList();

        var result = new PaginatedResult<GetVendorPaginatedListResponse>(responses)
        {
            CurrentPage = vendorsWithAppUsers.CurrentPage,
            TotalPages = vendorsWithAppUsers.TotalPages,
            TotalCount = vendorsWithAppUsers.TotalCount,
            PageSize = vendorsWithAppUsers.PageSize,
            Succeeded = vendorsWithAppUsers.Succeeded,
            Messages = vendorsWithAppUsers.Messages,
            Meta = new { Count = responses.Count }
        };
        
        return result;
    }
}
