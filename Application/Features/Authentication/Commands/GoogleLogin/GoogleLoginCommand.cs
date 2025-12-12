using Application.Common.Bases;

namespace Application.Features.Authentication.GoogleLogin;

public record GoogleLoginCommand(string IdToken) : IRequest<ApiResponse<JwtAuthResponse>>;

