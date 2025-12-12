using Application.Common.Bases;

namespace Application.Features.Authentication.AuthorizeUser;

public record AuthorizeUserQuery(string AccessToken) : IRequest<ApiResponse<string>>;

