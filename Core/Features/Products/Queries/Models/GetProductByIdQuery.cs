using Core.Features.Products.Queries.Responses;

namespace Core.Features.Products.Queries.Models
{
    public record GetProductByIdQuery(Guid ProductId, int ReviewPageNumber, int ReviewPageSize,
        ReviewSortingEnum SortBy, string? Search) : IRequest<ApiResponse<GetSingleProductResponse>>;
}
