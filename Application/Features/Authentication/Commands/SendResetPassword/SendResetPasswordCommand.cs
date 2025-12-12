using Application.Common.Bases;

namespace Application.Features.Authentication.SendResetPassword;

public record SendResetPasswordCommand(string Email) : IRequest<ApiResponse<string>>;

