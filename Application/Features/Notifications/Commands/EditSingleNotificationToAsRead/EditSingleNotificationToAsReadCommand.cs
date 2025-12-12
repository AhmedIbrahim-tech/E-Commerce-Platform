using Application.Common.Bases;

namespace Application.Features.Notifications.Commands.EditSingleNotificationToAsRead;

public record EditSingleNotificationToAsReadCommand(string notificationId) : IRequest<ApiResponse<string>>;

