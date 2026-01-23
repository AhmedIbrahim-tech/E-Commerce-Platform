using Application.Common.Bases;
using Application.Common.Errors;

namespace Application.Features.Notifications.Commands.EditAllNotificationsToAsRead;

public class EditAllNotificationsToAsReadCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService) : ApiResponseHandler(),
    IRequestHandler<EditAllNotificationsToAsReadCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(EditAllNotificationsToAsReadCommand request, CancellationToken cancellationToken)
    {
        if (!currentUserService.IsAuthenticated)
            return new ApiResponse<string>(UserErrors.InvalidJwtToken());

        var parsedUserId = currentUserService.GetUserId();
        var roles = await currentUserService.GetCurrentUserRolesAsync();
        var recipientRole = GetRecipientRole(roles);

        if (recipientRole == NotificationRecipientRole.Unknown)
        {
            return new ApiResponse<string>(UserErrors.InvalidJwtToken());
        }

        var unreadNotifications = await unitOfWork.Notifications.GetTableAsTracking()
            .Where(x => x.RecipientId == parsedUserId && x.RecipientRole == recipientRole && !x.IsRead)
            .ToListAsync(cancellationToken);

        if (unreadNotifications.Count == 0)
        {
            return Success("");
        }

        foreach (var notification in unreadNotifications)
        {
            notification.MarkAsRead();
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Success("");
    }

    private static NotificationRecipientRole GetRecipientRole(IReadOnlyCollection<string> roles)
    {
        if (roles.Contains(Roles.SuperAdmin))
            return NotificationRecipientRole.SuperAdmin;

        if (roles.Contains(Roles.Admin))
            return NotificationRecipientRole.Admin;

        if (roles.Contains(Roles.Merchant) || roles.Contains(Roles.Vendor))
            return NotificationRecipientRole.Merchant;

        if (roles.Contains(Roles.Customer))
            return NotificationRecipientRole.Customer;

        return NotificationRecipientRole.Unknown;
    }
}

