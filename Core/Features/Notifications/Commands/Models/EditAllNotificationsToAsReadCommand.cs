
namespace Core.Features.Notifications.Commands.Models
{
    public record EditAllNotificationsToAsReadCommand() : IRequest<ApiResponse<string>>;
}
