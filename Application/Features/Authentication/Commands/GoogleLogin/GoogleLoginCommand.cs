using Application.Common.Helpers;

namespace Application.Features.Authentication.Commands.GoogleLogin;

public record GoogleLoginCommand(string IdToken) : IRequest<ApiResponse<JwtAuthResponse>>;

