namespace Application.Features.Notifications.Queries.GetNotificationPaginatedList;

public record GetNotificationPaginatedListResponse
   (string Id,
    string? ReceiverId,
    string? Message,
    DateTimeOffset CreatedAt,
    bool IsRead);

