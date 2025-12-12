namespace API.Senders;

public class NotificationSender(IHubContext<NotificationsHub> hubContext) : INotificationSender
{
    public Task SendToUserAsync(string userId, string notification)
    {
        return hubContext.Clients.User(userId).SendAsync("ReceiveNotification", notification);
    }
}
