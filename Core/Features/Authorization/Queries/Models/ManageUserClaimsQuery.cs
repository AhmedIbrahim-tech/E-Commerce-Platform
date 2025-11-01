
namespace Core.Features.Authorization.Queries.Models
{
    public record ManageUserClaimsQuery(Guid UserId) : IRequest<ApiResponse<ManageUserClaimsResponse>>;
}
