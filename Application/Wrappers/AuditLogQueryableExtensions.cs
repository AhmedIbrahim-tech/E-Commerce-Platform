using Domain.Entities.AuditLogs;
using Domain.Enums.Sorting;

namespace Application.Wrappers;

public static class AuditLogQueryableExtensions
{
    public static IQueryable<AuditLog> ApplyFiltering(
        this IQueryable<AuditLog> query,
        AuditLogSortingEnum? sortBy,
        string? search,
        Guid? userId = null,
        string? eventType = null,
        DateTimeOffset? startDate = null,
        DateTimeOffset? endDate = null)
    {
        if (userId.HasValue)
        {
            query = query.Where(e => e.UserId == userId.Value);
        }

        if (!string.IsNullOrWhiteSpace(eventType))
        {
            query = query.Where(e => e.EventType == eventType);
        }

        if (startDate.HasValue)
        {
            query = query.Where(e => e.CreatedTime >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(e => e.CreatedTime <= endDate.Value);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(e =>
                (e.EventName != null && e.EventName.Contains(search)) ||
                (e.Description != null && e.Description.Contains(search)) ||
                (e.UserEmail != null && e.UserEmail.Contains(search)));
        }

        query = sortBy switch
        {
            AuditLogSortingEnum.CreatedAtDesc => query.OrderByDescending(e => e.CreatedTime),
            AuditLogSortingEnum.CreatedAtAsc => query.OrderBy(e => e.CreatedTime),
            AuditLogSortingEnum.EventTypeAsc => query.OrderBy(e => e.EventType),
            AuditLogSortingEnum.EventTypeDesc => query.OrderByDescending(e => e.EventType),
            AuditLogSortingEnum.EventNameAsc => query.OrderBy(e => e.EventName),
            AuditLogSortingEnum.EventNameDesc => query.OrderByDescending(e => e.EventName),
            _ => query.OrderByDescending(e => e.CreatedTime)
        };
        return query;
    }
}
