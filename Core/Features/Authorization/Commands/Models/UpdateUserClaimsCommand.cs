using Domain.Requests;

namespace Core.Features.Authorization.Commands.Models
{
    public class UpdateUserClaimsCommand : UpdateUserClaimsRequest, IRequest<ApiResponse<string>>
    {

    }
}
