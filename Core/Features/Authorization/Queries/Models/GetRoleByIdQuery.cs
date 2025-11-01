using Core.Features.Authorization.Queries.Responses;

namespace Core.Features.Authorization.Queries.Models
{
    public record GetRoleByIdQuery(Guid Id) : IRequest<ApiResponse<GetSingleRoleResponse>>;
}
