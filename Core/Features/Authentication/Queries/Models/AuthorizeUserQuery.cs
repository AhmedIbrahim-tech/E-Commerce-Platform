
namespace Core.Features.Authentication.Queries.Models
{
    public record AuthorizeUserQuery(string AccessToken) : IRequest<ApiResponse<string>>;
}
