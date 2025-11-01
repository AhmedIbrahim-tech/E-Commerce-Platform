
namespace Core.Features.Products.Commands.Models
{
    public record AddProductCommand
    (
        string? Name,
        string? Description,
        decimal? Price,
        int? StockQuantity,
        IFormFile? ImageURL,
        Guid CategoryId
    ) : IRequest<ApiResponse<string>>;
}
