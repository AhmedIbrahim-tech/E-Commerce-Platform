namespace Application.Features.Authentication.Commands.ResetPassword;

public record ResetPasswordCommand
    (string Email,
    string NewPassword,
    string ConfirmPassword) : IRequest<ApiResponse<string>>;

