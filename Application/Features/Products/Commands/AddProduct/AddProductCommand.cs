using Application.Common.Bases;

namespace Application.Features.Products.Commands.AddProduct;

public record AddProductCommand
(
    string? Name,
    string? Description,
    decimal? Price,
    int? StockQuantity,
    IFormFile? ImageURL,
    Guid CategoryId
) : IRequest<ApiResponse<string>>;

