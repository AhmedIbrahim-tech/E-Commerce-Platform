using Application.Common.Bases;

namespace Application.Features.Admins.Commands.DeleteAdmin;

public record DeleteAdminCommand(Guid Id) : IRequest<ApiResponse<string>>;
