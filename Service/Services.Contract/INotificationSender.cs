namespace Service.Services.Contract
{
    public interface INotificationSender
    {
        Task SendToUserAsync(string userId, string notification);
    }
}
