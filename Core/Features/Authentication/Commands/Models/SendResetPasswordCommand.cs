
namespace Core.Features.Authentication.Commands.Models
{
    public record SendResetPasswordCommand(string Email) : IRequest<ApiResponse<string>>;
}
