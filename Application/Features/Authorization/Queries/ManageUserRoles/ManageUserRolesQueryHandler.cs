using Application.Common.Bases;
using Application.Common.Errors;
using Domain.Responses;
using Infrastructure.Data.Identity;
using Infrastructure.RepositoriesHandlers.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Authorization.Queries.ManageUserRoles;

public class ManageUserRolesQueryHandler(
    RoleManager<AppRole> roleManager,
    UserManager<AppUser> userManager,
    IUnitOfWork unitOfWork) : ApiResponseHandler(),
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

        var userRoleIds = await (from ur in unitOfWork.Context.Set<Microsoft.AspNetCore.Identity.IdentityUserRole<Guid>>()
                                 where ur.UserId == user.Id
                                 select ur.RoleId)
                                 .ToListAsync(cancellationToken);

        var userRoleIdSet = userRoleIds.ToHashSet();

        foreach (var role in roles)
        {
            response.UserRoles.Add(new UserRoles
            {
                Id = role.Id,
                Name = role.Name,
                HasRole = userRoleIdSet.Contains(role.Id)
            });
        }

        return Success(response);
    }
}

