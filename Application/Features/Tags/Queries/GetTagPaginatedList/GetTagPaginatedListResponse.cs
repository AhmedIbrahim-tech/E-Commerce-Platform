namespace Application.Features.Tags.Queries.GetTagPaginatedList;

public record GetTagPaginatedListResponse(Guid Id, string Name, bool IsActive, DateTimeOffset CreatedTime);

