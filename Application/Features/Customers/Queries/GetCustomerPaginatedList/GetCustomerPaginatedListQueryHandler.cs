using Application.Common.Bases;
using Application.ServicesHandlers.Services;
using Infrastructure.Data.Identity;

namespace Application.Features.Customers.Queries.GetCustomerPaginatedList;

public class GetCustomerPaginatedListQueryHandler(
    ApplicationDbContext dbContext,
    UserManager<AppUser> userManager,
    IFileUploadService fileUploadService) : ApiResponseHandler(),
    IRequestHandler<GetCustomerPaginatedListQuery, PaginatedResult<GetCustomerPaginatedListResponse>>
{
    public async Task<PaginatedResult<GetCustomerPaginatedListResponse>> Handle(GetCustomerPaginatedListQuery request, CancellationToken cancellationToken)
    {
        var customersQuery = dbContext.Customers
            .AsQueryable()
            .ApplyFiltering(request.SortBy, request.Search);

        var customersWithAppUsers = await customersQuery
            .Join(userManager.Users,
                customer => customer.AppUserId,
                appUser => appUser.Id,
                (customer, appUser) => new
                {
                    Customer = customer,
                    AppUser = appUser
                })
            .ToPaginatedListAsync(request.PageNumber, request.PageSize);

        var userIds = customersWithAppUsers.Data.Select(x => x.AppUser.Id).ToList();
        var userRolesDict = new Dictionary<Guid, string>();
        
        foreach (var item in customersWithAppUsers.Data)
        {
            var roles = await userManager.GetRolesAsync(item.AppUser);
            userRolesDict[item.AppUser.Id] = roles.FirstOrDefault() ?? "Customer";
        }

        var responses = customersWithAppUsers.Data.Select(item => new GetCustomerPaginatedListResponse
        {
            Id = item.Customer.Id,
            FullName = item.Customer.FullName ?? string.Empty,
            Email = item.AppUser.Email ?? string.Empty,
            Gender = item.Customer.Gender,
            PhoneNumber = item.AppUser.PhoneNumber ?? string.Empty,
            Role = userRolesDict.GetValueOrDefault(item.AppUser.Id, "Customer"),
            ProfileImage = fileUploadService.ToAbsoluteUrl(item.AppUser.ProfileImage)
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

