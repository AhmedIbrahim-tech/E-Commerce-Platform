using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.Data;
using Infrastructure.Data.Identity;

namespace Application.Features.Employees.Queries.GetEmployeeById;

public class GetEmployeeByIdQueryHandler : ApiResponseHandler,
    IRequestHandler<GetEmployeeByIdQuery, ApiResponse<GetEmployeeByIdResponse>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;

    public GetEmployeeByIdQueryHandler(ApplicationDbContext dbContext, UserManager<AppUser> userManager) : base()
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<ApiResponse<GetEmployeeByIdResponse>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await _dbContext.Employees
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
        
        if (employee is null) return new ApiResponse<GetEmployeeByIdResponse>(EmployeeErrors.EmployeeNotFound());

        var appUser = await _userManager.FindByIdAsync(employee.AppUserId.ToString());
        if (appUser is null) return new ApiResponse<GetEmployeeByIdResponse>(UserErrors.UserNotFound());

        var employeeResponse = new GetEmployeeByIdResponse
        {
            Id = employee.Id,
            FullName = employee.FullName ?? string.Empty,
            Email = appUser.Email ?? string.Empty,
            PhoneNumber = appUser.PhoneNumber ?? string.Empty,
            Gender = employee.Gender,
            Position = employee.Position,
            Salary = employee.Salary,
            HireDate = employee.HireDate,
            Address = employee.Address
        };

        return Success(employeeResponse);
    }
}

