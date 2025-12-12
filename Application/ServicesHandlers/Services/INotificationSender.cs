namespace Application.ServicesHandlers.Services;

public interface INotificationSender
{
    Task SendToUserAsync(string userId, string notification);
}

