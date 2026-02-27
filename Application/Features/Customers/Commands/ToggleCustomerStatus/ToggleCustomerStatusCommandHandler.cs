using Application.Common.Bases;
using Application.Common.Errors;
using Application.ServicesHandlers.Auth;
using Infrastructure.Data.Identity;
using Infrastructure.RepositoriesHandlers.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Customers.Commands.ToggleCustomerStatus;

public class ToggleCustomerStatusCommandHandler(
    IUnitOfWork unitOfWork,
    UserManager<AppUser> userManager,
    ICurrentUserService currentUserService) : ApiResponseHandler(),
    IRequestHandler<ToggleCustomerStatusCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(ToggleCustomerStatusCommand request, CancellationToken cancellationToken)
    {
        var customer = await unitOfWork.Customers.GetTableAsTracking()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        
        if (customer is null) return new ApiResponse<string>(CustomerErrors.CustomerNotFound());

        var appUser = await userManager.FindByIdAsync(customer.AppUserId.ToString());
        if (appUser is null) return new ApiResponse<string>(UserErrors.UserNotFound());

        var currentUserId = currentUserService.GetUserId();
        
        if (customer.IsDeleted)
        {
            customer.Restore(currentUserId);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Success("Customer activated successfully");
        }
        else
        {
            customer.MarkDeleted(currentUserId);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Success("Customer deactivated successfully");
        }
    }
}
