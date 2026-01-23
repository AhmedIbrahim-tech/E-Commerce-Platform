using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.Data;
using Infrastructure.Data.Identity;

namespace Application.Features.Vendors.Commands.DeleteVendor;

public class DeleteVendorCommandHandler : ApiResponseHandler,
    IRequestHandler<DeleteVendorCommand, ApiResponse<string>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;

    public DeleteVendorCommandHandler(ApplicationDbContext dbContext, UserManager<AppUser> userManager) : base()
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<ApiResponse<string>> Handle(DeleteVendorCommand request, CancellationToken cancellationToken)
    {
        var vendor = await _dbContext.Vendors
            .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);
        
        if (vendor is null) return new ApiResponse<string>(VendorErrors.VendorNotFound());

        var appUser = await _userManager.FindByIdAsync(vendor.AppUserId.ToString());
        if (appUser != null)
        {
            var deleteResult = await _userManager.DeleteAsync(appUser);
            if (!deleteResult.Succeeded)
                return new ApiResponse<string>(VendorErrors.CannotDeleteVendorWithProducts());
        }

        vendor.MarkDeleted(vendor.AppUserId);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Deleted<string>();
    }
}
