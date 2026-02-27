using Application.Common.Bases;
using Application.Common.Errors;
using Domain.Entities.Users;
using Infrastructure.Data.Identity;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Admins.Commands.DeleteAdmin;

public class DeleteAdminCommandHandler(
    IUnitOfWork unitOfWork,
    UserManager<AppUser> userManager) : ApiResponseHandler(),
    IRequestHandler<DeleteAdminCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(DeleteAdminCommand request, CancellationToken cancellationToken)
    {
        var admin = await unitOfWork.Admins.GetTableAsTracking()
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
        
        if (admin is null) return new ApiResponse<string>(AdminErrors.AdminNotFound());

        var appUser = await userManager.FindByIdAsync(admin.AppUserId.ToString());
        if (appUser != null)
        {
            var deleteResult = await userManager.DeleteAsync(appUser);
            if (!deleteResult.Succeeded)
                return new ApiResponse<string>(AdminErrors.CannotDeleteAdmin());
        }

        await unitOfWork.Admins.DeleteAsync(admin, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Deleted<string>();
    }
}
