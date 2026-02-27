using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.Data.Identity;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Customers.Commands.DeleteCustomer;

public class DeleteCustomerCommandHandler(
    IUnitOfWork unitOfWork,
    UserManager<AppUser> userManager) : ApiResponseHandler(),
    IRequestHandler<DeleteCustomerCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await unitOfWork.Customers.GetTableAsTracking()
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        
        if (customer is null) return new ApiResponse<string>(CustomerErrors.CustomerNotFound());

        var appUser = await userManager.FindByIdAsync(customer.AppUserId.ToString());
        if (appUser != null)
        {
            var deleteResult = await userManager.DeleteAsync(appUser);
            if (!deleteResult.Succeeded)
                return new ApiResponse<string>(CustomerErrors.CannotDeleteCustomerWithOrders());
        }

        await unitOfWork.Customers.DeleteAsync(customer, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Deleted<string>();
    }
}

