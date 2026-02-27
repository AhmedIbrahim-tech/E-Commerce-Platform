using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.Data.Identity;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Vendors.Commands.DeleteVendor;

public class DeleteVendorCommandHandler(
    IUnitOfWork unitOfWork,
    UserManager<AppUser> userManager) : ApiResponseHandler(),
    IRequestHandler<DeleteVendorCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(DeleteVendorCommand request, CancellationToken cancellationToken)
    {
        var vendor = await unitOfWork.Vendors.GetTableAsTracking()
            .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);
        
        if (vendor is null) return new ApiResponse<string>(VendorErrors.VendorNotFound());

        var appUser = await userManager.FindByIdAsync(vendor.AppUserId.ToString());
        if (appUser != null)
        {
            var deleteResult = await userManager.DeleteAsync(appUser);
            if (!deleteResult.Succeeded)
                return new ApiResponse<string>(VendorErrors.CannotDeleteVendorWithProducts());
        }

        await unitOfWork.Vendors.DeleteAsync(vendor, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Deleted<string>();
    }
}
