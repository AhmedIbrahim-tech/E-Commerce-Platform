using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.Data.Identity;

namespace Application.Features.Authorization.Commands.EditRole;

public class EditRoleCommandHandler(RoleManager<AppRole> roleManager) : ApiResponseHandler(),
    IRequestHandler<EditRoleCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(EditRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await roleManager.FindByIdAsync(request.RoleId.ToString());
        if (role is null) return new ApiResponse<string>(RoleErrors.RoleNotFound());
        role.Name = request.RoleName;
        var result = await roleManager.UpdateAsync(role);
        if (result.Succeeded) return Edit("");
        return new ApiResponse<string>(RoleErrors.DuplicatedRoleName());
    }
}

