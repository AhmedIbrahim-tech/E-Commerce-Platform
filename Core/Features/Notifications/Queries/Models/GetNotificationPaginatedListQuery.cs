using Core.Features.Notifications.Queries.Responses;

namespace Core.Features.Notifications.Queries.Models
{
    public record GetNotificationPaginatedListQuery(int PageNumber, int PageSize) :
        IRequest<ApiResponse<PaginatedResult<GetNotificationPaginatedListResponse>>>;
}
