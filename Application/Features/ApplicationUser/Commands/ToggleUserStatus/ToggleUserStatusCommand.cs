using Application.Common.Bases;

namespace Application.Features.ApplicationUser.Commands.ToggleUserStatus;

public record ToggleUserStatusCommand(Guid Id, bool IsActive) : IRequest<ApiResponse<string>>;
