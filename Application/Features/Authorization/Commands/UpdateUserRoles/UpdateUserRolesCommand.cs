using Application.Common.Bases;

namespace Application.Features.Authorization.Commands.UpdateUserRoles;

public class UpdateUserRolesCommand : UpdateUserRolesRequest, IRequest<ApiResponse<string>>
{
}

