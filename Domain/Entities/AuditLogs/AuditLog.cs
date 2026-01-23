using Domain.Common.Base;

namespace Domain.Entities.AuditLogs;

public class AuditLog : BaseEntity
{
    public string EventType { get; private set; } = null!;
    public string EventName { get; private set; } = null!;
    public string? Description { get; private set; }
    
    public Guid? UserId { get; private set; }
    public string? UserEmail { get; private set; }
    
    public string? AdditionalData { get; private set; }
    
    public DateTimeOffset CreatedTime { get; private set; }

    private AuditLog() { }

    public AuditLog(string eventType, string eventName, string? description = null, Guid? userId = null, string? userEmail = null, string? additionalData = null)
    {
        EventType = eventType;
        EventName = eventName;
        Description = description;
        UserId = userId;
        UserEmail = userEmail;
        AdditionalData = additionalData;
        CreatedTime = DateTimeOffset.UtcNow;
    }
}
