using Application.Common.Bases;
using Application.ServicesHandlers.Services;
using Application.Wrappers;
using Infrastructure.Data.Identity;
using Infrastructure.RepositoriesHandlers.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Customers.Queries.GetCustomerPaginatedList;

public class GetCustomerPaginatedListQueryHandler(
    IUnitOfWork unitOfWork,
    UserManager<AppUser> userManager,
    IFileUploadService fileUploadService) : ApiResponseHandler(),
    IRequestHandler<GetCustomerPaginatedListQuery, PaginatedResult<GetCustomerPaginatedListResponse>>
{
    public async Task<PaginatedResult<GetCustomerPaginatedListResponse>> Handle(GetCustomerPaginatedListQuery request, CancellationToken cancellationToken)
    {
        var customersQuery = unitOfWork.Customers.GetTableNoTracking()
            .ApplyFiltering(request.SortBy, request.Search);

        var customersWithAppUsers = await customersQuery
            .Join(userManager.Users.AsNoTracking(),
                customer => customer.AppUserId,
                appUser => appUser.Id,
                (customer, appUser) => new
                {
                    Customer = customer,
                    AppUser = appUser
                })
            .ToPaginatedListAsync(request.PageNumber, request.PageSize);

        if (!customersWithAppUsers.Data.Any())
        {
            return new PaginatedResult<GetCustomerPaginatedListResponse>(new List<GetCustomerPaginatedListResponse>())
            {
                CurrentPage = request.PageNumber,
                TotalPages = 0,
                TotalCount = 0,
                PageSize = request.PageSize,
                Succeeded = true
            };
        }

        var userIds = customersWithAppUsers.Data.Select(x => x.AppUser.Id).ToList();
        
        var userRoleIds = await (from ur in unitOfWork.Context.Set<Microsoft.AspNetCore.Identity.IdentityUserRole<Guid>>()
                                  where userIds.Contains(ur.UserId)
                                  select new { ur.UserId, ur.RoleId })
                                  .ToListAsync(cancellationToken);

        var roleIds = userRoleIds.Select(x => x.RoleId).Distinct().ToList();
        var rolesDict = await unitOfWork.Context.Set<AppRole>()
            .AsNoTracking()
            .Where(r => roleIds.Contains(r.Id))
            .ToDictionaryAsync(r => r.Id, r => r.Name ?? "Customer", cancellationToken);

        var userRolesDict = userRoleIds
            .GroupBy(x => x.UserId)
            .ToDictionary(
                g => g.Key,
                g => rolesDict.GetValueOrDefault(g.First().RoleId, "Customer")
            );

        var responses = customersWithAppUsers.Data.Select(item => new GetCustomerPaginatedListResponse
        {
            Id = item.Customer.Id,
            FullName = item.Customer.FullName ?? string.Empty,
            Email = item.AppUser.Email ?? string.Empty,
            Gender = item.Customer.Gender,
            PhoneNumber = item.AppUser.PhoneNumber ?? string.Empty,
            Role = userRolesDict.GetValueOrDefault(item.AppUser.Id, "Customer"),
            ProfileImage = fileUploadService.ToAbsoluteUrl(item.AppUser.ProfileImage),
            IsDeleted = item.Customer.IsDeleted
        }).ToList();

        var result = new PaginatedResult<GetCustomerPaginatedListResponse>(responses)
        {
            CurrentPage = customersWithAppUsers.CurrentPage,
            TotalPages = customersWithAppUsers.TotalPages,
            TotalCount = customersWithAppUsers.TotalCount,
            PageSize = customersWithAppUsers.PageSize,
            Succeeded = customersWithAppUsers.Succeeded,
            Messages = customersWithAppUsers.Messages,
            Meta = new { Count = responses.Count }
        };
        
        return result;
    }
}

