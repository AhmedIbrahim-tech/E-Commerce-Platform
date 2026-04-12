using Application.Common.Helpers;

namespace Application.Features.Authentication.Commands.RefreshToken;

/// <param name="RefreshToken">Stored refresh token string (required).</param>
/// <param name="AccessToken">Optional access JWT; when sent, must match session jti and be expired before rotation.</param>
public record RefreshTokenCommand(string RefreshToken, string? AccessToken = null) : IRequest<ApiResponse<JwtAuthResponse>>;

