using Application.Common.Bases;
using Domain.Requests;

namespace Application.Features.Authorization.Commands.UpdateUserClaims;

public class UpdateUserClaimsCommand : UpdateUserClaimsRequest, IRequest<ApiResponse<string>>
{
}

