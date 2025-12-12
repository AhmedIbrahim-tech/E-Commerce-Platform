using Application.Common.Bases;

namespace Application.Features.Products.Commands.EditProduct;

public record EditProductCommand
(
    Guid Id,
    string? Name,
    string? Description,
    decimal? Price,
    int? StockQuantity,
    IFormFile? ImageURL,
    Guid CategoryId
) : IRequest<ApiResponse<string>>;

