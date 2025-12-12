using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Reviews.Commands.EditReview;

public class EditReviewCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService) : ApiResponseHandler(),
    IRequestHandler<EditReviewCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(EditReviewCommand request, CancellationToken cancellationToken)
    {
        var currentCustomerId = currentUserService.GetUserId();
        
        var review = await unitOfWork.Reviews.GetTableNoTracking()
            .Where(r => r.ProductId == request.ProductId && r.CustomerId == currentCustomerId)
            .FirstOrDefaultAsync(cancellationToken);

        if (review == null) return new ApiResponse<string>(ReviewErrors.ReviewNotFound());

        review.Rating = request.Rating;
        review.Comment = request.Comment;

        try
        {
            await unitOfWork.Reviews.UpdateAsync(review);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Edit("");
        }
        catch (Exception)
        {
            return new ApiResponse<string>(ReviewErrors.CannotModifyReview());
        }
    }
}

