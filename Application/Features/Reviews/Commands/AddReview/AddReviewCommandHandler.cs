using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Reviews.Commands.AddReview;

public class AddReviewCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService) : ApiResponseHandler(),
    IRequestHandler<AddReviewCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(AddReviewCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var review = new Review
            {
                ProductId = request.ProductId,
                Rating = request.Rating,
                Comment = request.Comment,
                CustomerId = currentUserService.GetUserId(),
                CreatedAt = DateTimeOffset.UtcNow.ToLocalTime()
            };

            await unitOfWork.Reviews.AddAsync(review);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Created("");
        }
        catch (Exception)
        {
            return new ApiResponse<string>(ReviewErrors.DuplicatedReview());
        }
    }
}

