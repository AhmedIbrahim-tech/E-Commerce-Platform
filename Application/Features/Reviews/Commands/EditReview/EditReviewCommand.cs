using Application.Common.Bases;

namespace Application.Features.Reviews.Commands.EditReview;

public record EditReviewCommand(Guid ProductId, Rating Rating, string? Comment) : IRequest<ApiResponse<string>>;

