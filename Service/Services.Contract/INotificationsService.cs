
namespace Service.Services.Contract
{
    public interface INotificationsService
    {
        Task AddNotificationAsync(NotificationResponse notification);
        IQueryable<NotificationResponse?> GetNotifications(string receiverId, NotificationReceiverType type);
        Task<string> MarkAllAsRead(string receiverId, NotificationReceiverType type);
        Task<string> MarkAsRead(string notificationId, string receiverId, NotificationReceiverType type);
    }
}
