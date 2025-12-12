using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.Data;
using Infrastructure.Data.Identity;

namespace Application.Features.Employees.Commands.DeleteEmployee;

public class DeleteEmployeeCommandHandler : ApiResponseHandler,
    IRequestHandler<DeleteEmployeeCommand, ApiResponse<string>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;

    public DeleteEmployeeCommandHandler(ApplicationDbContext dbContext, UserManager<AppUser> userManager) : base()
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<ApiResponse<string>> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _dbContext.Employees
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
        
        if (employee is null) return new ApiResponse<string>(EmployeeErrors.EmployeeNotFound());

        var appUser = await _userManager.FindByIdAsync(employee.AppUserId.ToString());
        if (appUser != null)
        {
            var deleteResult = await _userManager.DeleteAsync(appUser);
            if (!deleteResult.Succeeded)
                return new ApiResponse<string>(EmployeeErrors.CannotDeleteEmployeeWithOrders());
        }

        _dbContext.Employees.Remove(employee);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Deleted<string>();
    }
}

