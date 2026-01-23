using Application.Features.Notifications.Commands.EditSingleNotificationToAsRead;
using Application.Features.Notifications.Commands.EditAllNotificationsToAsRead;
using Application.Features.Notifications.Commands.MarkNotificationsRead;
using Application.Features.Notifications.Queries.GetNotificationPaginatedList;
using API.Controllers.Base;

namespace API.Controllers.Notifications
{
    [Authorize]
    public class NotificationsController : AppControllerBase
    {
        [HttpGet(Router.NotificationsRouting.List)]
        public async Task<IActionResult> GetNotifications([FromQuery] GetNotificationPaginatedListQuery query)
        {
            var response = await Mediator.Send(query);
            return NewResult(response);
        }

        [HttpGet(Router.NotificationsRouting.Paginated)]
        public async Task<IActionResult> GetNotificationPaginatedList([FromQuery] GetNotificationPaginatedListQuery query)
        {
            var response = await Mediator.Send(query);
            return NewResult(response);
        }

        [HttpPost(Router.NotificationsRouting.MarkRead)]
        public async Task<IActionResult> MarkRead([FromBody] MarkNotificationsReadCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut(Router.NotificationsRouting.MarkAsRead)]
        public async Task<IActionResult> MarkAsRead([FromRoute] string id)
        {
            var response = await Mediator.Send(new EditSingleNotificationToAsReadCommand(id));
            return NewResult(response);
        }

        [HttpPut(Router.NotificationsRouting.MarkAllAsRead)]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var response = await Mediator.Send(new EditAllNotificationsToAsReadCommand());
            return NewResult(response);
        }
    }
}
