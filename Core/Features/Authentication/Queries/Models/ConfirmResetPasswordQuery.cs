
namespace Core.Features.Authentication.Queries.Models
{
    public record ConfirmResetPasswordQuery(string Code, string Email) : IRequest<ApiResponse<string>>;
}
