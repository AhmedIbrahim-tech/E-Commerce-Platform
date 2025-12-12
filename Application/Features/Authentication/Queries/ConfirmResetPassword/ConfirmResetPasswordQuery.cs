using Application.Common.Bases;

namespace Application.Features.Authentication.ConfirmResetPassword;

public record ConfirmResetPasswordQuery(string Code, string Email) : IRequest<ApiResponse<string>>;

