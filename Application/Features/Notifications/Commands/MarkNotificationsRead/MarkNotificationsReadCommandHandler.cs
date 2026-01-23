namespace Application.Features.Notifications.Commands.MarkNotificationsRead;

public class MarkNotificationsReadCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService) : ApiResponseHandler(),
    IRequestHandler<MarkNotificationsReadCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(MarkNotificationsReadCommand request, CancellationToken cancellationToken)
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

        if (request.MarkAll)
        {
            var unreadNotifications = await unitOfWork.Notifications.GetTableAsTracking()
                .Where(x => x.RecipientId == parsedUserId && x.RecipientRole == recipientRole && !x.IsRead)
                .ToListAsync(cancellationToken);

            foreach (var notification in unreadNotifications)
            {
                notification.MarkAsRead();
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Success("");
        }

        if (!Guid.TryParse(request.Id, out var notificationId))
        {
            return new ApiResponse<string>(NotificationErrors.NotificationNotFound());
        }

        var notificationToUpdate = await unitOfWork.Notifications.GetTableAsTracking()
            .FirstOrDefaultAsync(x =>
                x.Id == notificationId &&
                x.RecipientId == parsedUserId &&
                x.RecipientRole == recipientRole, cancellationToken);

        if (notificationToUpdate == null)
        {
            return new ApiResponse<string>(NotificationErrors.NotificationNotFound());
        }

        if (notificationToUpdate.IsRead)
        {
            return new ApiResponse<string>(NotificationErrors.NotificationAlreadyRead());
        }

        notificationToUpdate.MarkAsRead();
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

