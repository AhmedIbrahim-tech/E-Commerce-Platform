using Application.Common.Bases;

namespace Application.Features.Warranties.Queries.GetWarrantyPaginatedList;

public record GetWarrantyPaginatedListQuery(
    int PageNumber,
    int PageSize,
    string? Search
) : IRequest<PaginatedResult<GetWarrantyPaginatedListResponse>>;
