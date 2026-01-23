namespace Application.Features.Units.Queries.GetUnitPaginatedList;

public record GetUnitPaginatedListResponse(Guid Id, string Name, string ShortName, string? Description, bool IsActive, DateTimeOffset CreatedTime);
