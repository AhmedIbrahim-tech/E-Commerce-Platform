namespace Application.Features.Units.Queries.GetUnitList;

public record GetUnitListResponse(Guid Id, string Name, string ShortName, string? Description, bool IsActive, DateTimeOffset CreatedTime);
