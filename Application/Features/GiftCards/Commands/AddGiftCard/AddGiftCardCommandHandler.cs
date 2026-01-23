namespace Application.Features.GiftCards.Commands.AddGiftCard;

public class AddGiftCardCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<AddGiftCardCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(AddGiftCardCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var giftCard = new GiftCard
            {
                Code = request.Code,
                RecipientName = request.RecipientName,
                RecipientEmail = request.RecipientEmail,
                Amount = request.Amount,
                RemainingAmount = request.Amount,
                ExpiryDate = request.ExpiryDate,
                IsActive = request.IsActive
            };

            await unitOfWork.GiftCards.AddAsync(giftCard, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Created("");
        }
        catch (Exception)
        {
            return new ApiResponse<string>(GiftCardErrors.DuplicatedGiftCardCode());
        }
    }
}
