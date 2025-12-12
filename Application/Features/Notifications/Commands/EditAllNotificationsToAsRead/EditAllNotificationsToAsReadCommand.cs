using Application.Common.Bases;

namespace Application.Features.Notifications.Commands.EditAllNotificationsToAsRead;

public record EditAllNotificationsToAsReadCommand() : IRequest<ApiResponse<string>>;

