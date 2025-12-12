using Application.Common.Bases;

namespace Application.Features.Authorization.Commands.AddRole;

public record AddRoleCommand(string RoleName) : IRequest<ApiResponse<string>>;

