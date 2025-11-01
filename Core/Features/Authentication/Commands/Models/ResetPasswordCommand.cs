
namespace Core.Features.Authentication.Commands.Models
{
    public record ResetPasswordCommand
        (string Email,
        string NewPassword,
        string ConfirmPassword) : IRequest<ApiResponse<string>>;
}
