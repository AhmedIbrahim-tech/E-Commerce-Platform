using Core.Features.Notifications.Commands.Models;

namespace Core.Features.Notifications.Commands.Handlers
{
    public class NotificationsCommandHandler : ApiResponseHandler,
        IRequestHandler<EditSingleNotificationToAsReadCommand, ApiResponse<string>>,
        IRequestHandler<EditAllNotificationsToAsReadCommand, ApiResponse<string>>
    {
        #region Fields
        private readonly INotificationsService _notificationsService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Constructors
        public NotificationsCommandHandler(
            INotificationsService notificationsService,
            IHttpContextAccessor httpContextAccessor) : base()
        {
            _notificationsService = notificationsService;
            _httpContextAccessor = httpContextAccessor;        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<string>> Handle(EditSingleNotificationToAsReadCommand request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null)
                return Unauthorized<string>(SharedResourcesKeys.UnAuthorized);

            var role = user?.FindFirst(ClaimTypes.Role)?.Value;
            var userId = user?.FindFirst(nameof(UserClaimModel.Id))?.Value;
            var result = role switch
            {
                "Admin" => await _notificationsService.MarkAsRead(request.notificationId, userId!, NotificationReceiverType.Admin),
                "Employee" => await _notificationsService.MarkAsRead(request.notificationId, userId!, NotificationReceiverType.Employee),
                "Customer" => await _notificationsService.MarkAsRead(request.notificationId, userId!, NotificationReceiverType.Customer),
                _ => await _notificationsService.MarkAsRead(request.notificationId, userId!, NotificationReceiverType.Unknowen),
            };

            if (result != "Success") return BadRequest<string>(SharedResourcesKeys.FailedToMarkNotifyAsRead);
            return Success("");
        }

        public async Task<ApiResponse<string>> Handle(EditAllNotificationsToAsReadCommand request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null)
                return Unauthorized<string>(SharedResourcesKeys.UnAuthorized);

            var role = user?.FindFirst(ClaimTypes.Role)?.Value;
            var userId = user?.FindFirst(nameof(UserClaimModel.Id))?.Value;
            var result = role switch
            {
                "Admin" => await _notificationsService.MarkAllAsRead(userId!, NotificationReceiverType.Admin),
                "Employee" => await _notificationsService.MarkAllAsRead(userId!, NotificationReceiverType.Employee),
                "Customer" => await _notificationsService.MarkAllAsRead(userId!, NotificationReceiverType.Customer),
                _ => await _notificationsService.MarkAllAsRead(userId!, NotificationReceiverType.Unknowen),
            };

            if (result != "Success") return BadRequest<string>(SharedResourcesKeys.FailedToMarkAllNotificationsAsRead);
            return Success("");
        }
        #endregion
    }
}
