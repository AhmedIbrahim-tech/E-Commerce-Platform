using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.GiftCards.Commands.DeleteGiftCard;

public class DeleteGiftCardCommandHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<DeleteGiftCardCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(DeleteGiftCardCommand request, CancellationToken cancellationToken)
    {
        var giftCard = await unitOfWork.GiftCards.GetTableNoTracking()
            .Where(gc => gc.Id.Equals(request.Id))
            .FirstOrDefaultAsync(cancellationToken);

        if (giftCard == null) return new ApiResponse<string>(GiftCardErrors.GiftCardNotFound());

        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await unitOfWork.GiftCards.DeleteAsync(giftCard, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return Deleted<string>();
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return BadRequest<string>("Cannot delete gift card");
        }
    }
}
