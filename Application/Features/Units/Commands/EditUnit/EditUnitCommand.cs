using Application.Common.Bases;

namespace Application.Features.Units.Commands.EditUnit;

public record EditUnitCommand(Guid Id, string Name, string ShortName, string? Description, bool IsActive) : IRequest<ApiResponse<string>>;
