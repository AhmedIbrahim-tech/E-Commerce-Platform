using Application.Common.Bases;

namespace Application.Features.Authentication.ConfirmEmail;

public record ConfirmEmailQuery(Guid UserId, string Code) : IRequest<ApiResponse<string>>;

