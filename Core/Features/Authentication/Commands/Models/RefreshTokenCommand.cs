
namespace Core.Features.Authentication.Commands.Models
{
    public record RefreshTokenCommand(string AccessToken, string RefreshToken) : IRequest<ApiResponse<JwtAuthResponse>>;
}
