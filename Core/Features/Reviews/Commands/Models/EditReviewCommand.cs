
namespace Core.Features.Reviews.Commands.Models
{
    public record EditReviewCommand(Guid ProductId, Rating Rating, string? Comment) : IRequest<ApiResponse<string>>;
}
