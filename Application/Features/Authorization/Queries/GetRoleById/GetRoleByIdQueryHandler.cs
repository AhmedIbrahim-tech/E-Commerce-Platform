using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.Data.Identity;

namespace Application.Features.Authorization.Queries.GetRoleById;

public class GetRoleByIdQueryHandler : ApiResponseHandler,
    IRequestHandler<GetRoleByIdQuery, ApiResponse<GetRoleByIdResponse>>
{
    private readonly RoleManager<AppRole> _roleManager;

    public GetRoleByIdQueryHandler(RoleManager<AppRole> roleManager) : base()
    {
        _roleManager = roleManager;
    }

    public async Task<ApiResponse<GetRoleByIdResponse>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<AppRole, GetRoleByIdResponse>> expression = role => new GetRoleByIdResponse
        {
            RoleId = role.Id,
            RoleName = role.Name!
        };

        var role = await _roleManager.Roles
            .Where(r => r.Id == request.Id)
            .Select(expression)
            .FirstOrDefaultAsync(cancellationToken);

        if (role is null) return new ApiResponse<GetRoleByIdResponse>(RoleErrors.RoleNotFound());
        return Success(role);
    }
}

