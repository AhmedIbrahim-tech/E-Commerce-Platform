using Application.Common.Bases;

namespace Application.Features.Authorization.Commands.EditRole;

public record EditRoleCommand(Guid RoleId, string RoleName) : IRequest<ApiResponse<string>>;

