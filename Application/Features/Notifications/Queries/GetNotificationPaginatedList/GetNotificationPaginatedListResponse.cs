namespace Application.Features.Notifications.Queries.GetNotificationPaginatedList;

public record GetNotificationPaginatedListResponse
   (Guid Id,
    string Type,
    System.Text.Json.JsonElement Data,
    bool IsRead,
    DateTimeOffset CreatedAt);

