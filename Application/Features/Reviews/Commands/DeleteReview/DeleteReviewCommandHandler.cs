using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Reviews.Commands.DeleteReview;

public class DeleteReviewCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService) : ApiResponseHandler(),
    IRequestHandler<DeleteReviewCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        var currentCustomerId = currentUserService.GetUserId();
        if (currentCustomerId == Guid.Empty) return new ApiResponse<string>(CustomerErrors.CustomerNotFound());
        
        var review = await unitOfWork.Reviews.GetTableNoTracking()
            .Where(r => r.ProductId == request.ProductId && r.CustomerId == currentCustomerId)
            .FirstOrDefaultAsync(cancellationToken);

        if (review == null) return new ApiResponse<string>(ReviewErrors.ReviewNotFound());

        try
        {
            await unitOfWork.Reviews.DeleteAsync(review);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Deleted<string>();
        }
        catch (Exception)
        {
            return new ApiResponse<string>(ReviewErrors.CannotDeleteReview());
        }
    }
}

