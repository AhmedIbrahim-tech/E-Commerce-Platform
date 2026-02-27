using Application.Common.Bases;
using Application.ServicesHandlers.Services;
using Application.Wrappers;
using Infrastructure.Data.Identity;
using Infrastructure.RepositoriesHandlers.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Vendors.Queries.GetVendorPaginatedList;

public class GetVendorPaginatedListQueryHandler(
    IUnitOfWork unitOfWork,
    UserManager<AppUser> userManager,
    IFileUploadService fileUploadService) : ApiResponseHandler(),
    IRequestHandler<GetVendorPaginatedListQuery, PaginatedResult<GetVendorPaginatedListResponse>>
{
    public async Task<PaginatedResult<GetVendorPaginatedListResponse>> Handle(GetVendorPaginatedListQuery request, CancellationToken cancellationToken)
    {
        var vendorsQuery = unitOfWork.Vendors.GetTableNoTracking()
            .IgnoreQueryFilters()
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

        if (!vendorsWithAppUsers.Data.Any())
        {
            return new PaginatedResult<GetVendorPaginatedListResponse>(new List<GetVendorPaginatedListResponse>())
            {
                CurrentPage = request.PageNumber,
                TotalPages = 0,
                TotalCount = 0,
                PageSize = request.PageSize,
                Succeeded = true
            };
        }

        var userIds = vendorsWithAppUsers.Data.Select(x => x.AppUser.Id).ToList();
        
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
