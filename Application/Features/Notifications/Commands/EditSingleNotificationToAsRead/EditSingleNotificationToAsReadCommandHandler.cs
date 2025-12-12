using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure;

namespace Application.Features.Notifications.Commands.EditSingleNotificationToAsRead;

public class EditSingleNotificationToAsReadCommandHandler(
    INotificationStore notificationStore,
    IHttpContextAccessor httpContextAccessor) : ApiResponseHandler(),
    IRequestHandler<EditSingleNotificationToAsReadCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(EditSingleNotificationToAsReadCommand request, CancellationToken cancellationToken)
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user == null)
            return new ApiResponse<string>(UserErrors.InvalidJwtToken());

        var role = user?.FindFirst(ClaimTypes.Role)?.Value;
        var userId = user?.FindFirst(nameof(UserClaimModel.Id))?.Value;
        var receiverType = role switch
        {
            "Admin" => NotificationReceiverType.Admin,
            "Employee" => NotificationReceiverType.Employee,
            "Customer" => NotificationReceiverType.Customer,
            _ => NotificationReceiverType.Unknowen,
        };

        try
        {
            await notificationStore.MarkAsRead(request.notificationId, userId!, receiverType);
            return Success("");
        }
        catch (Exception)
        {
            return new ApiResponse<string>(NotificationErrors.NotificationAlreadyRead());
        }
    }
}

