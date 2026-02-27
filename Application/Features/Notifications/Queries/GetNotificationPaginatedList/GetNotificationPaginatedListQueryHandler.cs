using Application.Common.Bases;
using Application.Common.Errors;
using Domain.Entities.Notifications;
using System.Text.Json;

namespace Application.Features.Notifications.Queries.GetNotificationPaginatedList;

public class GetNotificationPaginatedListQueryHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService) : ApiResponseHandler(),
    IRequestHandler<GetNotificationPaginatedListQuery, ApiResponse<PaginatedResult<GetNotificationPaginatedListResponse>>>
{
    public async Task<ApiResponse<PaginatedResult<GetNotificationPaginatedListResponse>>> Handle(GetNotificationPaginatedListQuery request, CancellationToken cancellationToken)
    {
        if (!currentUserService.IsAuthenticated)
            return new ApiResponse<PaginatedResult<GetNotificationPaginatedListResponse>>(UserErrors.InvalidJwtToken());

        var parsedUserId = currentUserService.GetUserId();
        var roles = await currentUserService.GetCurrentUserRolesAsync();
        var recipientRole = GetRecipientRole(roles);

        if (recipientRole == NotificationRecipientRole.Unknown)
        {
            return new ApiResponse<PaginatedResult<GetNotificationPaginatedListResponse>>(UserErrors.InvalidJwtToken());
        }

        var query = unitOfWork.Notifications.GetTableNoTracking()
            .Where(x => x.RecipientId == parsedUserId && x.RecipientRole == recipientRole)
            .OrderByDescending(x => x.CreatedAt);

        var unreadCount = await query.CountAsync(x => !x.IsRead, cancellationToken);

        Expression<Func<Notification, NotificationRow>> projection = x => new NotificationRow(
            x.Id,
            x.Type,
            x.Data,
            x.IsRead,
            x.CreatedAt);

        var paginatedRows = await query.Select(projection)
            .ToPaginatedListAsync(request.PageNumber, request.PageSize);

        var mapped = paginatedRows.Data.Select(x => new GetNotificationPaginatedListResponse(
            x.Id,
            x.Type,
            DeserializeData(x.Data),
            x.IsRead,
            x.CreatedAt)).ToList();

        var paginated = PaginatedResult<GetNotificationPaginatedListResponse>.Success(
            mapped,
            paginatedRows.TotalCount,
            paginatedRows.CurrentPage,
            paginatedRows.PageSize);

        return Success(paginated, meta: new { unreadCount });
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

    private static JsonElement DeserializeData(string? data)
    {
        try
        {
            return JsonSerializer.Deserialize<JsonElement>(string.IsNullOrWhiteSpace(data) ? "{}" : data);
        }
        catch
        {
            return JsonSerializer.Deserialize<JsonElement>("{}");
        }
    }

    private record NotificationRow(Guid Id, string Type, string Data, bool IsRead, DateTimeOffset CreatedAt);
}

