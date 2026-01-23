namespace Application.Features.GiftCards.Queries.GetGiftCardPaginatedList;

public record GetGiftCardPaginatedListQuery(int PageNumber, int PageSize, string? Search,
    GiftCardSortingEnum SortBy) : IRequest<PaginatedResult<GetGiftCardPaginatedListResponse>>;
