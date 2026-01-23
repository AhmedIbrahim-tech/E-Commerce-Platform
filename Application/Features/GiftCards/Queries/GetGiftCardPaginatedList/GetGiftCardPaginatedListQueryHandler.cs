using Application.Common.Bases;
using Domain.Entities.Promotions;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.GiftCards.Queries.GetGiftCardPaginatedList;

public class GetGiftCardPaginatedListQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetGiftCardPaginatedListQuery, PaginatedResult<GetGiftCardPaginatedListResponse>>
{
    public async Task<PaginatedResult<GetGiftCardPaginatedListResponse>> Handle(GetGiftCardPaginatedListQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<GiftCard, GetGiftCardPaginatedListResponse>> expression = gc => new GetGiftCardPaginatedListResponse(
            gc.Id,
            gc.Code,
            gc.RecipientName,
            gc.RecipientEmail,
            gc.Amount,
            gc.RemainingAmount,
            gc.ExpiryDate,
            gc.IsActive,
            gc.IsRedeemed,
            gc.CreatedTime
        );

        var queryable = unitOfWork.GiftCards.GetTableNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
            queryable = queryable.Where(gc => gc.Code.Contains(request.Search!) || 
                (gc.RecipientName != null && gc.RecipientName.Contains(request.Search!)) ||
                (gc.RecipientEmail != null && gc.RecipientEmail.Contains(request.Search!)));

        queryable = request.SortBy switch
        {
            GiftCardSortingEnum.CodeAsc => queryable.OrderBy(gc => gc.Code),
            GiftCardSortingEnum.CodeDesc => queryable.OrderByDescending(gc => gc.Code),
            GiftCardSortingEnum.CreatedTimeAsc => queryable.OrderBy(gc => gc.CreatedTime),
            GiftCardSortingEnum.CreatedTimeDesc => queryable.OrderByDescending(gc => gc.CreatedTime),
            _ => queryable.OrderBy(gc => gc.Code)
        };

        var paginatedList = await queryable.Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize);
        paginatedList.Meta = new { Count = paginatedList.Data.Count() };
        return paginatedList;
    }
}
