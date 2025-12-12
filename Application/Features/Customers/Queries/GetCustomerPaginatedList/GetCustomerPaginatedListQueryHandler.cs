using Application.Common.Bases;

namespace Application.Features.Customers.Queries.GetCustomerPaginatedList;

public class GetCustomerPaginatedListQueryHandler(ApplicationDbContext dbContext, UserManager<AppUser> userManager) : ApiResponseHandler(),
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
                (customer, appUser) => new GetCustomerPaginatedListResponse
                {
                    Id = customer.Id,
                    FullName = customer.FullName ?? string.Empty,
                    Email = appUser.Email ?? string.Empty,
                    Gender = customer.Gender,
                    PhoneNumber = appUser.PhoneNumber ?? string.Empty
                })
            .ToPaginatedListAsync(request.PageNumber, request.PageSize);

        customersWithAppUsers.Meta = new { Count = customersWithAppUsers.Data.Count() };
        return customersWithAppUsers;
    }
}

