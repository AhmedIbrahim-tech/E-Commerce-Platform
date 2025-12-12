using Application.Common.Bases;
using Application.Wrappers;
using Infrastructure.Data;
using Infrastructure.Data.Identity;

namespace Application.Features.Employees.Commands.EditEmployee;

public class EditEmployeeCommandHandler : ApiResponseHandler,
    IRequestHandler<EditEmployeeCommand, ApiResponse<string>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;

    public EditEmployeeCommandHandler(ApplicationDbContext dbContext, UserManager<AppUser> userManager) : base()
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<ApiResponse<string>> Handle(EditEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _dbContext.Employees
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
        
        if (employee is null) return NotFound<string>();

        var appUser = await _userManager.FindByIdAsync(employee.AppUserId.ToString());
        if (appUser is null) return NotFound<string>("AppUser not found");

        var isUserNameDuplicate = await _userManager.UserNameExistsAsync(request.UserName, employee.AppUserId);
        if (isUserNameDuplicate)
            return BadRequest<string>("Username already exists");

        var isEmailDuplicate = await _userManager.EmailExistsAsync(request.Email, employee.AppUserId);
        if (isEmailDuplicate)
            return BadRequest<string>("Email already exists");

        // Update AppUser properties
        appUser.UserName = request.UserName;
        appUser.Email = request.Email;
        appUser.PhoneNumber = request.PhoneNumber;
        appUser.FullName = $"{request.FirstName} {request.LastName}".Trim();

        // Update Employee properties
        employee.FullName = $"{request.FirstName} {request.LastName}".Trim();
        employee.Gender = request.Gender;

        // Update Employee specific properties
        employee.Position = request.Position;
        employee.Salary = request.Salary;
        employee.Address = request.Address;

        var updateAppUserResult = await _userManager.UpdateAsync(appUser);
        if (!updateAppUserResult.Succeeded)
            return BadRequest<string>("Update failed");

        await _dbContext.SaveChangesAsync(cancellationToken);
        return Edit("");
    }
}

