namespace Application.Features.Authentication.Commands.LogoutAll;

public record LogoutAllCommand() : IRequest<ApiResponse<string>>;
