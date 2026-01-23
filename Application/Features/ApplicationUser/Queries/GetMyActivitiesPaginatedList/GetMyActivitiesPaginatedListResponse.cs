namespace Application.Features.ApplicationUser.Queries.GetMyActivitiesPaginatedList;

public class GetMyActivitiesPaginatedListResponse
{
    public Guid Id { get; set; }
    public string Category { get; set; } = string.Empty;
    public string ActionType { get; set; } = string.Empty;
    public string RelatedEntity { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? AdditionalData { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

