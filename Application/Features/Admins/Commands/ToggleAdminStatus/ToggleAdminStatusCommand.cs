using Application.Common.Bases;
using MediatR;

namespace Application.Features.Admins.Commands.ToggleAdminStatus;

public record ToggleAdminStatusCommand(Guid Id) : IRequest<ApiResponse<string>>;
