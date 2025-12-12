using Application.Common.Bases;

namespace Application.Features.Authentication.ResetPassword;

public record ResetPasswordCommand
    (string Email,
    string NewPassword,
    string ConfirmPassword) : IRequest<ApiResponse<string>>;

