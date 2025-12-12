using Application.Common.Bases;

namespace Application.Features.Authorization.Commands.DeleteRole;

public record DeleteRoleCommand(Guid RoleId) : IRequest<ApiResponse<string>>;

