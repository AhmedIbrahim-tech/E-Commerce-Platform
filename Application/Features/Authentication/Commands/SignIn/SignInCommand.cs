using Application.Common.Bases;

namespace Application.Features.Authentication.SignIn;

public record SignInCommand(string UserName, string Password) : IRequest<ApiResponse<JwtAuthResponse>>;

