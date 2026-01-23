using Domain.Enums.Sorting;

namespace Application.Features.Vendors.Queries.GetVendorPaginatedList;

public record GetVendorPaginatedListQuery(int PageNumber, int PageSize, string? Search,
    VendorSortingEnum SortBy) : IRequest<PaginatedResult<GetVendorPaginatedListResponse>>;
