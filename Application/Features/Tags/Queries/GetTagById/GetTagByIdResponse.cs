namespace Application.Features.Tags.Queries.GetTagById;

public record GetTagByIdResponse(Guid Id, string Name, bool IsActive, DateTimeOffset CreatedTime, DateTimeOffset? ModifiedTime);

