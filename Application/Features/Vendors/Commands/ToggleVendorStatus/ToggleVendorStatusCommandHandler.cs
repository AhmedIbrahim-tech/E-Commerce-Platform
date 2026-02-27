using Application.Common.Bases;
using Application.Common.Errors;
using Application.ServicesHandlers.Auth;
using Infrastructure.Data.Identity;
using Infrastructure.RepositoriesHandlers.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Vendors.Commands.ToggleVendorStatus;

public class ToggleVendorStatusCommandHandler(
    IUnitOfWork unitOfWork,
    UserManager<AppUser> userManager,
    ICurrentUserService currentUserService) : ApiResponseHandler(),
    IRequestHandler<ToggleVendorStatusCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(ToggleVendorStatusCommand request, CancellationToken cancellationToken)
    {
        var vendor = await unitOfWork.Vendors.GetTableAsTracking()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);
        
        if (vendor is null) return new ApiResponse<string>(VendorErrors.VendorNotFound());

        var appUser = await userManager.FindByIdAsync(vendor.AppUserId.ToString());
        if (appUser is null) return new ApiResponse<string>(UserErrors.UserNotFound());

        var currentUserId = currentUserService.GetUserId();
        
        if (vendor.IsDeleted)
        {
            vendor.Restore(currentUserId);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Success("Vendor activated successfully");
        }
        else
        {
            vendor.MarkDeleted(currentUserId);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Success("Vendor deactivated successfully");
        }
    }
}
