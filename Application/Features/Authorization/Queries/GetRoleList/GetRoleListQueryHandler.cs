using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.Data.Identity;

namespace Application.Features.Authorization.Queries.GetRoleList;

public class GetRoleListQueryHandler : ApiResponseHandler,
    IRequestHandler<GetRoleListQuery, ApiResponse<List<GetRoleListResponse>>>
{
    private readonly RoleManager<AppRole> _roleManager;

    public GetRoleListQueryHandler(RoleManager<AppRole> roleManager) : base()
    {
        _roleManager = roleManager;
    }

    public async Task<ApiResponse<List<GetRoleListResponse>>> Handle(GetRoleListQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<AppRole, GetRoleListResponse>> expression = role => new GetRoleListResponse
        {
            RoleId = role.Id,
            RoleName = role.Name
        };

        var roleList = await _roleManager.Roles
            .Select(expression)
            .ToListAsync(cancellationToken);

        if (roleList is null || !roleList.Any()) return new ApiResponse<List<GetRoleListResponse>>(RoleErrors.RoleNotFound());
        return Success(roleList);
    }
}

