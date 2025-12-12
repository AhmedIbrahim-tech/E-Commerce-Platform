using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.Data;
using Infrastructure.Data.Identity;

namespace Application.Features.Authorization.Commands.UpdateUserRoles;

public class UpdateUserRolesCommandHandler(
    UserManager<AppUser> userManager,
    ApplicationDbContext dbContext) : ApiResponseHandler(),
    IRequestHandler<UpdateUserRolesCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
    {
        var transact = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var user = await userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                await transact.RollbackAsync(cancellationToken);
                return new ApiResponse<string>(UserErrors.UserNotFound());
            }

            var userRoles = await userManager.GetRolesAsync(user);
            var removeResult = await userManager.RemoveFromRolesAsync(user, userRoles);
            if (!removeResult.Succeeded)
            {
                await transact.RollbackAsync(cancellationToken);
                return new ApiResponse<string>(RoleErrors.RoleNotAssigned());
            }

            var selectedRoles = request.UserRoles.Where(x => x.HasRole == true).Select(x => x.Name);
            var addRolesresult = await userManager.AddToRolesAsync(user, selectedRoles);
            if (!addRolesresult.Succeeded)
            {
                await transact.RollbackAsync(cancellationToken);
                return new ApiResponse<string>(RoleErrors.InvalidPermissions());
            }

            await transact.CommitAsync(cancellationToken);
            return Edit("");
        }
        catch (Exception)
        {
            await transact.RollbackAsync(cancellationToken);
            return new ApiResponse<string>(RoleErrors.RoleAlreadyAssigned());
        }
    }
}

