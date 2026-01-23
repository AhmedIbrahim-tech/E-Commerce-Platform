namespace Application.Features.Notifications.Commands.MarkNotificationsRead;

public record MarkNotificationsReadCommand(string? Id, bool MarkAll = false) : IRequest<ApiResponse<string>>;

