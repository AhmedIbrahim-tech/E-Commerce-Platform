using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure;

namespace Application.Features.Notifications.Queries.GetNotificationPaginatedList;

public class GetNotificationPaginatedListQueryHandler(
    INotificationStore notificationStore,
    IHttpContextAccessor httpContextAccessor) : ApiResponseHandler(),
    IRequestHandler<GetNotificationPaginatedListQuery, ApiResponse<PaginatedResult<GetNotificationPaginatedListResponse>>>
{
    public async Task<ApiResponse<PaginatedResult<GetNotificationPaginatedListResponse>>> Handle(GetNotificationPaginatedListQuery request, CancellationToken cancellationToken)
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user == null)
            return new ApiResponse<PaginatedResult<GetNotificationPaginatedListResponse>>(UserErrors.InvalidJwtToken());

        var role = user?.FindFirst(ClaimTypes.Role)?.Value;
        var userId = user?.FindFirst(nameof(UserClaimModel.Id))?.Value;
        var receiverType = role switch
        {
            "Admin" => NotificationReceiverType.Admin,
            "Employee" => NotificationReceiverType.Employee,
            "Customer" => NotificationReceiverType.Customer,
            _ => NotificationReceiverType.Unknowen,
        };

        var notifications = notificationStore.GetNotifications(userId!, receiverType).AsQueryable();

        Expression<Func<NotificationResponse, GetNotificationPaginatedListResponse>> expression = c => new GetNotificationPaginatedListResponse(
            c.Id,
            c.ReceiverId,
            c.Message,
            c.CreatedAt,
            c.IsRead
        );

        var paginatedList = await notifications.Select(expression)
                                               .ToPaginatedListAsync(request.PageNumber, request.PageSize);
        return Success(paginatedList);
    }
}

