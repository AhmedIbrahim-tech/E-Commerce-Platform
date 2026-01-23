namespace Application.Features.Discounts.Queries.GetDiscountPaginatedList;

public record GetDiscountPaginatedListQuery(int PageNumber, int PageSize, string? Search,
    DiscountSortingEnum SortBy) : IRequest<PaginatedResult<GetDiscountPaginatedListResponse>>;
