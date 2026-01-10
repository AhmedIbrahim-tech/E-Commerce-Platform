using Application.Common.Helpers;

namespace Application.Features.Authentication.Commands.SignIn;

public record SignInCommand(string UserName, string Password) : IRequest<ApiResponse<JwtAuthResponse>>;

