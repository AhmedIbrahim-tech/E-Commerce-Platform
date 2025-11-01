
namespace Core.Features.Authorization.Commands.Models
{
    public record EditRoleCommand(Guid RoleId, string RoleName) : IRequest<ApiResponse<string>>;
}
