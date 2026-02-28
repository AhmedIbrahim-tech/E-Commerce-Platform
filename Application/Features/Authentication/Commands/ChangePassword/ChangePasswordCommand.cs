namespace Application.Features.Authentication.Commands.ChangePassword;

public record ChangePasswordCommand(string CurrentPassword, string NewPassword, string ConfirmPassword) : IRequest<ApiResponse<string>>;
