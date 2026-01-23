namespace Application.Features.Coupons.Queries.GetCouponPaginatedList;

public record GetCouponPaginatedListQuery(int PageNumber, int PageSize, string? Search,
    CouponSortingEnum SortBy) : IRequest<PaginatedResult<GetCouponPaginatedListResponse>>;
