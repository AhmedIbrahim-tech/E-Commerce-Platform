using Application.Common.Bases;
using Application.Common.Errors;
using Application.Wrappers;
using Infrastructure.Data;
using Infrastructure.Data.Identity;

namespace Application.Features.Customers.Commands.EditCustomer;

public class EditCustomerCommandHandler : ApiResponseHandler,
    IRequestHandler<EditCustomerCommand, ApiResponse<string>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;

    public EditCustomerCommandHandler(ApplicationDbContext dbContext, UserManager<AppUser> userManager) : base()
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<ApiResponse<string>> Handle(EditCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _dbContext.Customers
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        
        if (customer is null) return new ApiResponse<string>(CustomerErrors.CustomerNotFound());

        var appUser = await _userManager.FindByIdAsync(customer.AppUserId.ToString());
        if (appUser is null) return new ApiResponse<string>(UserErrors.UserNotFound());

        var isUserNameDuplicate = await _userManager.UserNameExistsAsync(request.UserName!, customer.AppUserId);
        if (isUserNameDuplicate)
            return new ApiResponse<string>(UserErrors.DuplicatedEmail());

        var isEmailDuplicate = await _userManager.EmailExistsAsync(request.Email!, customer.AppUserId);
        if (isEmailDuplicate)
            return new ApiResponse<string>(CustomerErrors.DuplicatedPhoneNumber());

        // Update AppUser properties
        appUser.UserName = request.UserName;
        appUser.Email = request.Email;
        appUser.PhoneNumber = request.PhoneNumber;
        appUser.FullName = $"{request.FirstName} {request.LastName}".Trim();

        // Update Customer properties
        customer.FullName = $"{request.FirstName} {request.LastName}".Trim();
        customer.Gender = request.Gender;

        var updateAppUserResult = await _userManager.UpdateAsync(appUser);
        if (!updateAppUserResult.Succeeded)
            return new ApiResponse<string>(CustomerErrors.InvalidCustomerData());

        await _dbContext.SaveChangesAsync(cancellationToken);
        return Edit("");
    }
}

