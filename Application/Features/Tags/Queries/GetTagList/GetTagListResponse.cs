namespace Application.Features.Tags.Queries.GetTagList;

public record GetTagListResponse(Guid Id, string Name, bool IsActive, DateTimeOffset CreatedTime);

