
namespace Service.Services
{
    public class NotificationsService : INotificationsService
    {
        private readonly INotificationStore _notificationStore;
        private readonly INotificationSender _notificationSender;

        public NotificationsService(
            INotificationStore notificationStore,
            INotificationSender notificationSender)
        {
            _notificationStore = notificationStore;
            _notificationSender = notificationSender;
        }

        public async Task AddNotificationAsync(NotificationResponse notification)
        {
            var result = await _notificationStore.AddNotification(notification);
            if (result == "Success")
                await _notificationSender.SendToUserAsync(notification.ReceiverId!, notification.Message!);
        }

        public IQueryable<NotificationResponse?> GetNotifications(string receiverId, NotificationReceiverType type)
        {
            return _notificationStore.GetNotifications(receiverId, type).AsQueryable();
        }

        public async Task<string> MarkAsRead(string notificationId, string receiverId, NotificationReceiverType type)
        {
            try
            {
                await _notificationStore.MarkAsRead(notificationId, receiverId, type);
                return "Success";
            }
            catch (Exception)
            {
                return "Failed";
            }
        }

        public async Task<string> MarkAllAsRead(string receiverId, NotificationReceiverType type)
        {
            try
            {
                await _notificationStore.MarkAllAsRead(receiverId, type);
                return "Success";
            }
            catch (Exception)
            {
                return "Failed";
            }
        }
    }
}
