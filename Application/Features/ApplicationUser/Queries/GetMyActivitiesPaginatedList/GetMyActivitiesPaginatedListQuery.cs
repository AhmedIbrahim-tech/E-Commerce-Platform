using Application.Common.Bases;
using Application.Wrappers;
using Domain.Enums.Sorting;

namespace Application.Features.ApplicationUser.Queries.GetMyActivitiesPaginatedList;

public record GetMyActivitiesPaginatedListQuery(
    int PageNumber,
    int PageSize,
    string? Search,
    string? Category = null,
    AuditLogSortingEnum? SortBy = null,
    DateTimeOffset? StartDate = null,
    DateTimeOffset? EndDate = null) : IRequest<PaginatedResult<GetMyActivitiesPaginatedListResponse>>;

