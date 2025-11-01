using Domain.Requests;

namespace Core.Features.Authorization.Commands.Models
{
    public class UpdateUserRolesCommand : UpdateUserRolesRequest, IRequest<ApiResponse<string>>
    {

    }
}
