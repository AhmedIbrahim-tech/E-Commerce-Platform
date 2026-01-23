using Application.Common.Bases;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.GiftCards.Queries.GetGiftCardList;

public class GetGiftCardListQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetGiftCardListQuery, ApiResponse<List<GetGiftCardListResponse>>>
{
    public async Task<ApiResponse<List<GetGiftCardListResponse>>> Handle(GetGiftCardListQuery request, CancellationToken cancellationToken)
    {
        var giftCardList = await unitOfWork.GiftCards.GetTableNoTracking()
            .Select(gc => new GetGiftCardListResponse(
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
            ))
            .ToListAsync(cancellationToken);

        return Success(giftCardList);
    }
}
