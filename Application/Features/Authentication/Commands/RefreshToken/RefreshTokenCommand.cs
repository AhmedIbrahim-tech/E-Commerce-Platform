using Application.Common.Helpers;

namespace Application.Features.Authentication.Commands.RefreshToken;

public record RefreshTokenCommand(string AccessToken, string RefreshToken) : IRequest<ApiResponse<JwtAuthResponse>>;

