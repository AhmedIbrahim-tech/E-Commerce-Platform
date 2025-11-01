
namespace Core.Features.Authorization.Commands.Models
{
    public record AddRoleCommand(string RoleName) : IRequest<ApiResponse<string>>;
}
