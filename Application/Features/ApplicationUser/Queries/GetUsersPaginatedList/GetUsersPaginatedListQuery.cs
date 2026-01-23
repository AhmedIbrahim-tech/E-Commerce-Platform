using Application.Common.Bases;

namespace Application.Features.ApplicationUser.Queries.GetUsersPaginatedList;

public record GetUsersPaginatedListQuery(
    int PageNumber,
    int PageSize,
    string? Search = null,
    string? Role = null,
    bool? IsActive = null,
    int? SortBy = null) : IRequest<PaginatedResult<GetUsersPaginatedListResponse>>;
