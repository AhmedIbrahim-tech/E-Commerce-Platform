namespace Application.Features.Units.Queries.GetUnitById;

public record GetUnitByIdResponse(Guid Id, string Name, string ShortName, string? Description, bool IsActive, DateTimeOffset CreatedTime);
