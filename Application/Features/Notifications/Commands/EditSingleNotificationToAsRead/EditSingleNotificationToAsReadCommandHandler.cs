using Application.Common.Bases;
using Application.Common.Errors;

namespace Application.Features.Notifications.Commands.EditSingleNotificationToAsRead;

public class EditSingleNotificationToAsReadCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService) : ApiResponseHandler(),
    IRequestHandler<EditSingleNotificationToAsReadCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(EditSingleNotificationToAsReadCommand request, CancellationToken cancellationToken)
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

        if (!Guid.TryParse(request.notificationId, out var notificationId))
        {
            return new ApiResponse<string>(NotificationErrors.NotificationNotFound());
        }

        var notification = await unitOfWork.Notifications.GetTableAsTracking()
            .FirstOrDefaultAsync(x =>
                x.Id == notificationId &&
                x.RecipientId == parsedUserId &&
                x.RecipientRole == recipientRole, cancellationToken);

        if (notification == null)
        {
            return new ApiResponse<string>(NotificationErrors.NotificationNotFound());
        }

        if (notification.IsRead)
        {
            return new ApiResponse<string>(NotificationErrors.NotificationAlreadyRead());
        }

        notification.MarkAsRead();
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Success("");
    }

    private static NotificationRecipientRole GetRecipientRole(IReadOnlyCollection<string> roles)
    {
        if (roles.Contains(Roles.SuperAdmin))
            return NotificationRecipientRole.SuperAdmin;

        if (roles.Contains(Roles.Admin))
            return NotificationRecipientRole.Admin;

        if (roles.Contains(Roles.Merchant) || roles.Contains(Roles.StaffMerchant))
            return NotificationRecipientRole.Merchant;

        if (roles.Contains(Roles.Customer))
            return NotificationRecipientRole.Customer;

        return NotificationRecipientRole.Unknown;
    }
}

