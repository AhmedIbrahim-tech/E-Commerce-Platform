using Application.Common.Bases;
using Application.Common.Errors;
using Application.ServicesHandlers.Auth;
using Infrastructure.Data.Identity;
using Infrastructure.RepositoriesHandlers.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Admins.Commands.ToggleAdminStatus;

public class ToggleAdminStatusCommandHandler(
    IUnitOfWork unitOfWork,
    UserManager<AppUser> userManager,
    ICurrentUserService currentUserService) : ApiResponseHandler(),
    IRequestHandler<ToggleAdminStatusCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(ToggleAdminStatusCommand request, CancellationToken cancellationToken)
    {
        var admin = await unitOfWork.Admins.GetTableAsTracking()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
        
        if (admin is null) return new ApiResponse<string>(AdminErrors.AdminNotFound());

        var appUser = await userManager.FindByIdAsync(admin.AppUserId.ToString());
        if (appUser is null) return new ApiResponse<string>(UserErrors.UserNotFound());

        var currentUserId = currentUserService.GetUserId();
        
        if (admin.IsDeleted)
        {
            admin.Restore(currentUserId);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Success("Admin activated successfully");
        }
        else
        {
            admin.MarkDeleted(currentUserId);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Success("Admin deactivated successfully");
        }
    }
}
