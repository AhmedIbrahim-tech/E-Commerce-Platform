
namespace Core.Features.Emails.Commands.Models
{
    public record SendEmailCommand(string Email, string ReturnUrl, EmailType EmailType) : IRequest<ApiResponse<string>>;
}
