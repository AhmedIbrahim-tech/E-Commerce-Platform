
namespace Core.Features.Authentication.Queries.Models
{
    public record ConfirmEmailQuery(Guid UserId, string Code) : IRequest<ApiResponse<string>>;
}
