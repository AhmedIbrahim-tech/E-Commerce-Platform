
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Repositories
{
    public class NotificationStore : INotificationStore
    {
        private readonly IMemoryCache _memoryCache;
        private const string NotificationKeyPrefix = "notifications:";

        public NotificationStore(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<string> AddNotification(NotificationResponse notification)
        {
            try
            {
                var key = $"{NotificationKeyPrefix}{notification.ReceiverType}:{notification.ReceiverId}";
                var existingNotifications = GetNotifications(notification.ReceiverId, notification.ReceiverType);
                
                // Add new notification to the beginning of the list
                existingNotifications.Insert(0, notification);
                
                // Keep only the last 100 notifications to avoid large cache entries
                if (existingNotifications.Count > 100)
                {
                    existingNotifications = existingNotifications.Take(100).ToList();
                }

                var serialized = JsonSerializer.Serialize(existingNotifications);

                var options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30)
                };

                _memoryCache.Set(key, serialized, options);
                return "Success";
            }
            catch (Exception)
            {
                return "Failed";
            }
        }

        public List<NotificationResponse> GetNotifications(string? receiverId, NotificationReceiverType type)
        {
            try
            {
                var key = $"{NotificationKeyPrefix}{type}:{receiverId}";
                
                if (!_memoryCache.TryGetValue(key, out string? cached))
                {
                    return new List<NotificationResponse>();
                }

                var notifications = JsonSerializer.Deserialize<List<NotificationResponse>>(cached ?? "");
                
                return notifications?
                    .Where(n => n != null)
                    .OrderByDescending(n => n!.CreatedAt)
                    .ToList() ?? new List<NotificationResponse>();
            }
            catch
            {
                return new List<NotificationResponse>();
            }
        }

        public async Task MarkAsRead(string notificationId, string receiverId, NotificationReceiverType type)
        {
            var key = $"{NotificationKeyPrefix}{type}:{receiverId}";
            var notifications = GetNotifications(receiverId, type);

            var notification = notifications.FirstOrDefault(n => n.Id == notificationId);
            if (notification != null)
            {
                notification.IsRead = true;

                var serialized = JsonSerializer.Serialize(notifications);

                var options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30)
                };

                _memoryCache.Set(key, serialized, options);
            }
        }

        public async Task MarkAllAsRead(string receiverId, NotificationReceiverType type)
        {
            var key = $"{NotificationKeyPrefix}{type}:{receiverId}";
            var notifications = GetNotifications(receiverId, type);

            foreach (var notification in notifications.Where(n => !n.IsRead))
            {
                notification.IsRead = true;
            }

            var serialized = JsonSerializer.Serialize(notifications);

            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30)
            };

            _memoryCache.Set(key, serialized, options);
        }
    }
}
