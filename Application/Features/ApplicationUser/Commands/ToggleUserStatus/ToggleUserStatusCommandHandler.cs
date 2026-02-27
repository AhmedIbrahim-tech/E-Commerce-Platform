using Application.Common.Bases;
using Application.Common.Errors;
using Application.ServicesHandlers.Auth;
using Infrastructure.Data.Identity;
using Infrastructure.RepositoriesHandlers.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ApplicationUser.Commands.ToggleUserStatus;

public class ToggleUserStatusCommandHandler(
    IUnitOfWork unitOfWork,
    UserManager<AppUser> userManager,
    ICurrentUserService currentUserService) : ApiResponseHandler(),
    IRequestHandler<ToggleUserStatusCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(ToggleUserStatusCommand request, CancellationToken cancellationToken)
    {
        var appUserId = await (from admin in unitOfWork.Admins.GetTableNoTracking()
                                where admin.Id == request.Id
                                select admin.AppUserId)
                                .Union(from vendor in unitOfWork.Vendors.GetTableNoTracking()
                                       where vendor.Id == request.Id
                                       select vendor.AppUserId)
                                .Union(from customer in unitOfWork.Customers.GetTableNoTracking()
                                       where customer.Id == request.Id
                                       select customer.AppUserId)
                                .FirstOrDefaultAsync(cancellationToken);

        if (appUserId == Guid.Empty)
        {
            appUserId = request.Id;
        }

        var appUser = await userManager.FindByIdAsync(appUserId.ToString());
        if (appUser == null)
            return NotFound<string>("User not found");

        if (request.IsActive)
        {
            appUser.LockoutEnd = null;
        }
        else
        {
            appUser.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100);
        }

        var result = await userManager.UpdateAsync(appUser);
        if (!result.Succeeded)
            return BadRequest<string>("Failed to update user status");

        return Success("User status updated successfully");
    }
}
