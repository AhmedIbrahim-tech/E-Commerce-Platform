using Application.Common.Bases;

namespace Application.Features.Products.Commands.DeleteProduct;

public record DeleteProductCommand(Guid ProductId) : IRequest<ApiResponse<string>>;

