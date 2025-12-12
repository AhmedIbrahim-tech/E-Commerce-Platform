using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.Data.Identity;

namespace Application.Features.Authorization.Commands.AddRole;

public class AddRoleCommandHandler(RoleManager<AppRole> roleManager) : ApiResponseHandler(),
    IRequestHandler<AddRoleCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(AddRoleCommand request, CancellationToken cancellationToken)
    {
        var identityRole = new AppRole();
        identityRole.Name = request.RoleName;
        var result = await roleManager.CreateAsync(identityRole);
        if (result.Succeeded) return Created("");
        return new ApiResponse<string>(RoleErrors.DuplicatedRoleName());
    }
}

