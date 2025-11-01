
namespace Core.Features.Authorization.Commands.Models
{
    public record DeleteRoleCommand(Guid RoleId) : IRequest<ApiResponse<string>>;
}
