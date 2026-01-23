namespace Application.Features.Authentication.Commands.Logout;

public record LogoutCommand(string RefreshToken) : IRequest<ApiResponse<string>>;
