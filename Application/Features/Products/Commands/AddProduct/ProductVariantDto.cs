namespace Application.Features.Products.Commands.AddProduct;

public record ProductVariantDto(
    string VariantAttribute,
    string VariantValue,
    string SKU,
    int Quantity,
    decimal Price,
    IFormFile? ImageURL
);
