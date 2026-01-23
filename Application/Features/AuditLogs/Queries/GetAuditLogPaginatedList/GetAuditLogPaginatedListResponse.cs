namespace Application.Features.AuditLogs.Queries.GetAuditLogPaginatedList;

public record GetAuditLogPaginatedListResponse
{
    public Guid Id { get; init; }
    public string EventType { get; init; } = null!;
    public string EventName { get; init; } = null!;
    public string? Description { get; init; }
    public Guid? UserId { get; init; }
    public string? UserEmail { get; init; }
    public string? AdditionalData { get; init; }
    public DateTimeOffset CreatedTime { get; init; }
}
