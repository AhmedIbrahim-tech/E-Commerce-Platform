using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.GiftCards.Commands.EditGiftCard;

public class EditGiftCardCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<EditGiftCardCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(EditGiftCardCommand request, CancellationToken cancellationToken)
    {
        var giftCard = await unitOfWork.GiftCards.GetTableNoTracking()
            .Where(gc => gc.Id.Equals(request.Id))
            .FirstOrDefaultAsync(cancellationToken);

        if (giftCard == null) return new ApiResponse<string>(GiftCardErrors.GiftCardNotFound());

        giftCard.Code = request.Code;
        giftCard.RecipientName = request.RecipientName;
        giftCard.RecipientEmail = request.RecipientEmail;
        giftCard.Amount = request.Amount;
        giftCard.ExpiryDate = request.ExpiryDate;
        giftCard.IsActive = request.IsActive;

        try
        {
            await unitOfWork.GiftCards.UpdateAsync(giftCard, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Edit("");
        }
        catch (Exception)
        {
            return new ApiResponse<string>(GiftCardErrors.DuplicatedGiftCardCode());
        }
    }
}
