namespace Application.Features.Authentication.Commands.SignIn;

public record SignInCommand(string Email, string Password, bool RememberMe = false) : IRequest<ApiResponse<JwtAuthResponse>>;

