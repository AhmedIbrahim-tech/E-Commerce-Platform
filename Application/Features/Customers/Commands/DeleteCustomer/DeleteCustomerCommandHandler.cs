using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.Data;
using Infrastructure.Data.Identity;

namespace Application.Features.Customers.Commands.DeleteCustomer;

public class DeleteCustomerCommandHandler : ApiResponseHandler,
    IRequestHandler<DeleteCustomerCommand, ApiResponse<string>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;

    public DeleteCustomerCommandHandler(ApplicationDbContext dbContext, UserManager<AppUser> userManager) : base()
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<ApiResponse<string>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _dbContext.Customers
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        
        if (customer is null) return new ApiResponse<string>(CustomerErrors.CustomerNotFound());

        var appUser = await _userManager.FindByIdAsync(customer.AppUserId.ToString());
        if (appUser != null)
        {
            var deleteResult = await _userManager.DeleteAsync(appUser);
            if (!deleteResult.Succeeded)
                return new ApiResponse<string>(CustomerErrors.CannotDeleteCustomerWithOrders());
        }

        _dbContext.Customers.Remove(customer);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Deleted<string>();
    }
}

