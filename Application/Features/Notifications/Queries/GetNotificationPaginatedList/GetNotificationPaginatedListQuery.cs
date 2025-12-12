using Application.Common.Bases;

namespace Application.Features.Notifications.Queries.GetNotificationPaginatedList;

public record GetNotificationPaginatedListQuery(int PageNumber, int PageSize) :
    IRequest<ApiResponse<PaginatedResult<GetNotificationPaginatedListResponse>>>;

