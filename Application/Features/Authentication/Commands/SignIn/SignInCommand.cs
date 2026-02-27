namespace Application.Features.Authentication.Commands.SignIn;

public record SignInCommand(string Email, string Password) : IRequest<ApiResponse<JwtAuthResponse>>;

