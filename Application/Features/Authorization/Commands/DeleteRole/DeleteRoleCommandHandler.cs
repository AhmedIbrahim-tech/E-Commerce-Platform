using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.Data.Identity;

namespace Application.Features.Authorization.Commands.DeleteRole;

public class DeleteRoleCommandHandler(
    RoleManager<AppRole> roleManager,
    UserManager<AppUser> userManager) : ApiResponseHandler(),
    IRequestHandler<DeleteRoleCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await roleManager.FindByIdAsync(request.RoleId.ToString());
        if (role is null) return new ApiResponse<string>(RoleErrors.RoleNotFound());
        var users = await userManager.GetUsersInRoleAsync(role.Name!);
        if (users is null || users.Count < 0)
        {
            var result = await roleManager.DeleteAsync(role);
            if (result.Succeeded) return Deleted<string>();
            return new ApiResponse<string>(RoleErrors.CannotDeleteSystemRole());
        }
        return new ApiResponse<string>(RoleErrors.CannotDeleteRoleWithUsers());
    }
}

