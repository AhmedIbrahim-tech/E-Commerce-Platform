using Application.Common.Bases;

namespace Application.Features.Units.Commands.AddUnit;

public record AddUnitCommand(string Name, string ShortName, string? Description, bool IsActive) : IRequest<ApiResponse<string>>;
