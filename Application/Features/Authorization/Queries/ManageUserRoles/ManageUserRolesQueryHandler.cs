using Application.Common.Bases;
using Application.Common.Errors;
using Domain.Responses;
using Infrastructure.Data.Identity;

namespace Application.Features.Authorization.Queries.ManageUserRoles;

public class ManageUserRolesQueryHandler(
    RoleManager<AppRole> roleManager,
    UserManager<AppUser> userManager) : ApiResponseHandler(),
    IRequestHandler<ManageUserRolesQuery, ApiResponse<ManageUserRolesResponse>>
{
    public async Task<ApiResponse<ManageUserRolesResponse>> Handle(ManageUserRolesQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null) return new ApiResponse<ManageUserRolesResponse>(UserErrors.UserNotFound());

        var response = new ManageUserRolesResponse();
        var roles = await roleManager.Roles.ToListAsync(cancellationToken);
        response.UserId = user.Id;
        response.UserRoles = new List<UserRoles>();

        foreach (var role in roles)
        {
            var hasRole = await userManager.IsInRoleAsync(user, role.Name!);
            response.UserRoles.Add(new UserRoles
            {
                Id = role.Id,
                Name = role.Name,
                HasRole = hasRole
            });
        }

        return Success(response);
    }
}

