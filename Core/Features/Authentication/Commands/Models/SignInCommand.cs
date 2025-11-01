
namespace Core.Features.Authentication.Commands.Models
{
    public record SignInCommand(string UserName, string Password) : IRequest<ApiResponse<JwtAuthResponse>>;
}
