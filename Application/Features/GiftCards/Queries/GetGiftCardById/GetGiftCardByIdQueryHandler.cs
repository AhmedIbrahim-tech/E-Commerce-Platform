using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.GiftCards.Queries.GetGiftCardById;

public class GetGiftCardByIdQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetGiftCardByIdQuery, ApiResponse<GetGiftCardByIdResponse>>
{
    public async Task<ApiResponse<GetGiftCardByIdResponse>> Handle(GetGiftCardByIdQuery request, CancellationToken cancellationToken)
    {
        var giftCard = await unitOfWork.GiftCards.GetTableNoTracking()
            .Where(gc => gc.Id == request.Id)
            .Select(gc => new GetGiftCardByIdResponse(
                gc.Id,
                gc.Code,
                gc.RecipientName,
                gc.RecipientEmail,
                gc.Amount,
                gc.RemainingAmount,
                gc.ExpiryDate,
                gc.IsActive,
                gc.IsRedeemed,
                gc.RedeemedDate,
                gc.CreatedTime
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (giftCard == null)
            return NotFound<GetGiftCardByIdResponse>("Gift card not found");

        return Success(giftCard);
    }
}
