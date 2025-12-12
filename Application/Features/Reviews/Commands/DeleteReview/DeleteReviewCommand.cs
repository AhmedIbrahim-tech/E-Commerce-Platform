using Application.Common.Bases;

namespace Application.Features.Reviews.Commands.DeleteReview;

public record DeleteReviewCommand(Guid ProductId) : IRequest<ApiResponse<string>>;

