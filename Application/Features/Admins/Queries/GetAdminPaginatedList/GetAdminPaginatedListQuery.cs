using Application.Common.Bases;
using Application.Wrappers;
using Domain.Enums.Sorting;

namespace Application.Features.Admins.Queries.GetAdminPaginatedList;

public record GetAdminPaginatedListQuery(int PageNumber, int PageSize, string? Search,
    AdminSortingEnum SortBy) : IRequest<PaginatedResult<GetAdminPaginatedListResponse>>;
