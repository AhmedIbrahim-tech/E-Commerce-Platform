using Application.Common.Bases;
using Application.Wrappers;
using Domain.Enums.Sorting;

namespace Application.Features.AuditLogs.Queries.GetAuditLogPaginatedList;

public record GetAuditLogPaginatedListQuery(
    int PageNumber, 
    int PageSize, 
    string? Search,
    AuditLogSortingEnum? SortBy = null,
    Guid? UserId = null,
    string? EventType = null,
    DateTimeOffset? StartDate = null,
    DateTimeOffset? EndDate = null) : IRequest<PaginatedResult<GetAuditLogPaginatedListResponse>>;
