using Application.Common.Bases;

namespace Application.Features.Reviews.Commands.AddReview;

public record AddReviewCommand(Guid ProductId, Rating Rating, string? Comment) : IRequest<ApiResponse<string>>;

