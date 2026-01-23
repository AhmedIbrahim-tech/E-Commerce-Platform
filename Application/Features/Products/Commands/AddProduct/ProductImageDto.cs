namespace Application.Features.Products.Commands.AddProduct;

public record ProductImageDto(
    IFormFile ImageFile,
    bool IsPrimary = false,
    int DisplayOrder = 0
);
