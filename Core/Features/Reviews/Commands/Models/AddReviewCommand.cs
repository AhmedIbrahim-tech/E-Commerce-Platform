
namespace Core.Features.Reviews.Commands.Models
{
    public record AddReviewCommand(Guid ProductId, Rating Rating, string? Comment) : IRequest<ApiResponse<string>>;
}
