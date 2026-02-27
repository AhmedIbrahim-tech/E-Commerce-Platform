using Application.Common.Bases;
using Application.ServicesHandlers.Auth;
using Application.Wrappers;
using Infrastructure.RepositoriesHandlers.UnitOfWork;
using System.Text.Json;

namespace Application.Features.ApplicationUser.Queries.GetMyActivitiesPaginatedList;

public class GetMyActivitiesPaginatedListQueryHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService) : ApiResponseHandler(),
    IRequestHandler<GetMyActivitiesPaginatedListQuery, PaginatedResult<GetMyActivitiesPaginatedListResponse>>
{
    public async Task<PaginatedResult<GetMyActivitiesPaginatedListResponse>> Handle(GetMyActivitiesPaginatedListQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetUserId();
        var eventType = NormalizeCategory(request.Category);

        var auditLogsQuery = unitOfWork.AuditLogs.GetTableNoTracking()
            .ApplyFiltering(
                request.SortBy,
                request.Search,
                userId,
                eventType,
                request.StartDate,
                request.EndDate);

        var auditLogs = await auditLogsQuery
            .Select(log => new GetMyActivitiesPaginatedListResponse
            {
                Id = log.Id,
                Category = log.EventType,
                ActionType = log.EventName,
                RelatedEntity = log.EventType,
                Description = log.Description,
                AdditionalData = log.AdditionalData,
                CreatedAt = log.CreatedTime
            })
            .ToPaginatedListAsync(request.PageNumber, request.PageSize);

        foreach (var item in auditLogs.Data)
        {
            item.RelatedEntity = ResolveRelatedEntityFromAdditionalData(item.AdditionalData, item.Category);
        }

        auditLogs.Meta = new { Count = auditLogs.Data.Count() };
        return auditLogs;
    }

    private static string ResolveRelatedEntityFromAdditionalData(string? additionalData, string fallback)
    {
        if (string.IsNullOrWhiteSpace(additionalData))
            return fallback;

        try
        {
            using var doc = JsonDocument.Parse(additionalData);
            if (doc.RootElement.ValueKind == JsonValueKind.Object)
            {
                if (doc.RootElement.TryGetProperty("entityType", out var entityTypeProp) &&
                    entityTypeProp.ValueKind == JsonValueKind.String)
                {
                    return entityTypeProp.GetString() ?? fallback;
                }

                if (doc.RootElement.TryGetProperty("entity", out var entityProp) &&
                    entityProp.ValueKind == JsonValueKind.String)
                {
                    return entityProp.GetString() ?? fallback;
                }
            }
        }
        catch
        {
            return fallback;
        }

        return fallback;
    }

    private static string? NormalizeCategory(string? category)
    {
        if (string.IsNullOrWhiteSpace(category))
            return null;

        var c = category.Trim();

        return c.Equals("Orders", StringComparison.OrdinalIgnoreCase) ? "Orders"
            : c.Equals("Profile", StringComparison.OrdinalIgnoreCase) ? "Profile"
            : c.Equals("Documents", StringComparison.OrdinalIgnoreCase) ? "Documents"
            : c.Equals("Security", StringComparison.OrdinalIgnoreCase) ? "Security"
            : c;
    }
}

