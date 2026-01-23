using Application.Common.Bases;
using Application.Common.Errors;
using Application.ServicesHandlers.Auth;
using Domain.Entities.Users;
using Infrastructure.Data;
using Infrastructure.Data.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Admins.Commands.ToggleAdminStatus;

public class ToggleAdminStatusCommandHandler : ApiResponseHandler,
    IRequestHandler<ToggleAdminStatusCommand, ApiResponse<string>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;
    private readonly ICurrentUserService _currentUserService;

    public ToggleAdminStatusCommandHandler(
        ApplicationDbContext dbContext, 
        UserManager<AppUser> userManager,
        ICurrentUserService currentUserService) : base()
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _currentUserService = currentUserService;
    }

    public async Task<ApiResponse<string>> Handle(ToggleAdminStatusCommand request, CancellationToken cancellationToken)
    {
        var admin = await _dbContext.Admins
            .IgnoreQueryFilters()
            .AsTracking()
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
        
        if (admin is null) return new ApiResponse<string>(AdminErrors.AdminNotFound());

        var appUser = await _userManager.FindByIdAsync(admin.AppUserId.ToString());
        if (appUser is null) return new ApiResponse<string>(UserErrors.UserNotFound());

        var currentUserId = _currentUserService.GetUserId();
        
        // Toggle status: if deleted, restore; if active, mark as deleted
        if (admin.IsDeleted)
        {
            admin.Restore(currentUserId);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Success("Admin activated successfully");
        }
        else
        {
            admin.MarkDeleted(currentUserId);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Success("Admin deactivated successfully");
        }
    }
}
