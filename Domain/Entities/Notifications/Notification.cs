using Domain.Common.Base;

namespace Domain.Entities.Notifications;

public class Notification : BaseEntity
{
    public string Type { get; private set; } = string.Empty;
    public NotificationRecipientRole RecipientRole { get; private set; } = NotificationRecipientRole.Unknown;
    public Guid RecipientId { get; private set; }
    public string Data { get; private set; } = "{}";
    public bool IsRead { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;

    private Notification()
    {
    }

    public Notification(string type, NotificationRecipientRole recipientRole, Guid recipientId, string data)
    {
        SetType(type);
        SetRecipient(recipientRole, recipientId);
        SetData(data);
        IsRead = false;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public void MarkAsRead()
    {
        IsRead = true;
    }

    private void SetType(string type)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new DomainException("Notification type is required");

        Type = type.Trim();
    }

    private void SetRecipient(NotificationRecipientRole recipientRole, Guid recipientId)
    {
        if (recipientRole == NotificationRecipientRole.Unknown)
            throw new DomainException("Notification recipient role is required");

        if (recipientId == Guid.Empty)
            throw new DomainException("Notification recipient id is required");

        RecipientRole = recipientRole;
        RecipientId = recipientId;
    }

    private void SetData(string data)
    {
        Data = string.IsNullOrWhiteSpace(data) ? "{}" : data.Trim();
    }
}

