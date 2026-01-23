using Application.Common.Bases;

namespace Application.Features.Units.Commands.DeleteUnit;

public record DeleteUnitCommand(Guid Id) : IRequest<ApiResponse<string>>;
