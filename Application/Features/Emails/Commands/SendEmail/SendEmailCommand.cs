using Application.Common.Bases;

namespace Application.Features.Emails.Commands.SendEmail;

public record SendEmailCommand(string Email, string ReturnUrl, EmailType EmailType) : IRequest<ApiResponse<string>>;

