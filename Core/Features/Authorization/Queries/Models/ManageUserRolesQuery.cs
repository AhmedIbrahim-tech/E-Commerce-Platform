
namespace Core.Features.Authorization.Queries.Models
{
    public record ManageUserRolesQuery(Guid UserId) : IRequest<ApiResponse<ManageUserRolesResponse>>;
}
