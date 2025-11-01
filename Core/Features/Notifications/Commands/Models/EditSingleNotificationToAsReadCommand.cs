
namespace Core.Features.Notifications.Commands.Models
{
    public record EditSingleNotificationToAsReadCommand(string notificationId) : IRequest<ApiResponse<string>>;
}
