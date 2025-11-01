using Core.Features.Authorization.Queries.Responses;

namespace Core.Features.Authorization.Queries.Models
{
    public record GetRoleListQuery : IRequest<ApiResponse<List<GetRoleListResponse>>>;
}
