using Application.Common.Bases;
using Application.Wrappers;
using Infrastructure.Data;
using Infrastructure.Data.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Employees.Queries.GetEmployeePaginatedList;

public class GetEmployeePaginatedListQueryHandler : ApiResponseHandler,
    IRequestHandler<GetEmployeePaginatedListQuery, PaginatedResult<GetEmployeePaginatedListResponse>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;

    public GetEmployeePaginatedListQueryHandler(ApplicationDbContext dbContext, UserManager<AppUser> userManager) : base()
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<PaginatedResult<GetEmployeePaginatedListResponse>> Handle(GetEmployeePaginatedListQuery request, CancellationToken cancellationToken)
    {
        var employeesQuery = _dbContext.Employees
            .AsQueryable()
            .ApplyFiltering(request.SortBy, request.Search);

        var employeesWithAppUsers = await employeesQuery
            .Join(_userManager.Users,
                employee => employee.AppUserId,
                appUser => appUser.Id,
                (employee, appUser) => new GetEmployeePaginatedListResponse
                {
                    Id = employee.Id,
                    FullName = employee.FullName ?? string.Empty,
                    Email = appUser.Email ?? string.Empty,
                    Gender = employee.Gender,
                    PhoneNumber = appUser.PhoneNumber ?? string.Empty,
                    Position = employee.Position,
                    Salary = employee.Salary,
                    HireDate = employee.HireDate,
                    Address = employee.Address
                })
            .ToPaginatedListAsync(request.PageNumber, request.PageSize);

        employeesWithAppUsers.Meta = new { Count = employeesWithAppUsers.Data.Count() };
        return employeesWithAppUsers;
    }
}

