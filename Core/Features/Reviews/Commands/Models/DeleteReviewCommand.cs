
namespace Core.Features.Reviews.Commands.Models
{
    public record DeleteReviewCommand(Guid ProductId) : IRequest<ApiResponse<string>>;
}
