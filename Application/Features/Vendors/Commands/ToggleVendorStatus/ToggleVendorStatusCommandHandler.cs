using Application.Common.Bases;
using Application.Common.Errors;
using Application.ServicesHandlers.Auth;
using Domain.Entities.Users;
using Infrastructure.Data;
using Infrastructure.Data.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Vendors.Commands.ToggleVendorStatus;

public class ToggleVendorStatusCommandHandler : ApiResponseHandler,
    IRequestHandler<ToggleVendorStatusCommand, ApiResponse<string>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;
    private readonly ICurrentUserService _currentUserService;

    public ToggleVendorStatusCommandHandler(
        ApplicationDbContext dbContext, 
        UserManager<AppUser> userManager,
        ICurrentUserService currentUserService) : base()
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _currentUserService = currentUserService;
    }

    public async Task<ApiResponse<string>> Handle(ToggleVendorStatusCommand request, CancellationToken cancellationToken)
    {
        var vendor = await _dbContext.Vendors
            .IgnoreQueryFilters()
            .AsTracking()
            .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);
        
        if (vendor is null) return new ApiResponse<string>(VendorErrors.VendorNotFound());

        var appUser = await _userManager.FindByIdAsync(vendor.AppUserId.ToString());
        if (appUser is null) return new ApiResponse<string>(UserErrors.UserNotFound());

        var currentUserId = _currentUserService.GetUserId();
        
        // Toggle status: if deleted, restore; if active, mark as deleted
        if (vendor.IsDeleted)
        {
            vendor.Restore(currentUserId);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Success("Vendor activated successfully");
        }
        else
        {
            vendor.MarkDeleted(currentUserId);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Success("Vendor deactivated successfully");
        }
    }
}
