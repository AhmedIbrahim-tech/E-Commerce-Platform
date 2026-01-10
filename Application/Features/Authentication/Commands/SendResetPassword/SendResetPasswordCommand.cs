namespace Application.Features.Authentication.Commands.SendResetPassword;

public record SendResetPasswordCommand(string Email) : IRequest<ApiResponse<string>>;

