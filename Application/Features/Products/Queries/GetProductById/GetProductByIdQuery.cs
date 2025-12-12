using Application.Common.Bases;

namespace Application.Features.Products.Queries.GetProductById;

public record GetProductByIdQuery(Guid ProductId, int ReviewPageNumber, int ReviewPageSize,
    ReviewSortingEnum SortBy, string? Search) : IRequest<ApiResponse<GetProductByIdResponse>>;

