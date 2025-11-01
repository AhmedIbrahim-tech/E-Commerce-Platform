
namespace Core.Features.Authentication.Commands.Models
{
    public record GoogleLoginCommand(string IdToken) : IRequest<ApiResponse<JwtAuthResponse>>;
}
