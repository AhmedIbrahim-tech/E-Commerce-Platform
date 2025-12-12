using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.Data.Identity;

namespace Application.Features.Employees.Commands.AddEmployee;

public class AddEmployeeCommandHandler : ApiResponseHandler,
    IRequestHandler<AddEmployeeCommand, ApiResponse<string>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ApplicationDbContext _dbContext;

    public AddEmployeeCommandHandler(UserManager<AppUser> userManager, ApplicationDbContext dbContext) : base()
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }

    public async Task<ApiResponse<string>> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
    {
        // Check if email exists in Identity
        var existingAppUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingAppUser != null) return new ApiResponse<string>(EmployeeErrors.DuplicatedEmployeeEmail());

        var userByUserName = await _userManager.FindByNameAsync(request.UserName);
        if (userByUserName != null) return new ApiResponse<string>(UserErrors.DuplicatedEmail());

        // Step 1: Create Identity user (AppUser)
        var appUser = new AppUser
        {
            UserName = request.UserName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            EmailConfirmed = true
        };

        var createResult = await _userManager.CreateAsync(appUser, request.Password);
        if (!createResult.Succeeded)
            return new ApiResponse<string>(EmployeeErrors.InvalidEmployeeData());

        // Step 2: Create domain entity (Employee) with FK to AppUser
        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            AppUserId = appUser.Id,
            FullName = $"{request.FirstName} {request.LastName}".Trim(),
            Gender = request.Gender,
            Position = request.Position,
            Salary = request.Salary,
            Address = request.Address,
            HireDate = DateTimeOffset.UtcNow
        };

        await _dbContext.Employees.AddAsync(employee, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Step 3: Add default role "Employee"
        var addToRoleResult = await _userManager.AddToRoleAsync(appUser, "Employee");
        if (!addToRoleResult.Succeeded)
            return new ApiResponse<string>(RoleErrors.InvalidPermissions());

        // Step 4: Add default employee policies
        var claims = new List<Claim>
        {
            new Claim("Edit Employee", "True"),
            new Claim("Get Employee", "True")
        };
        var addDefaultClaimsResult = await _userManager.AddClaimsAsync(appUser, claims);
        if (!addDefaultClaimsResult.Succeeded)
            return new ApiResponse<string>(PermissionErrors.PermissionNotAssigned());

        return Created("");
    }
}

