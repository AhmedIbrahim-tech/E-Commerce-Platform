using Core.Features.Reviews.Queries.Responses;

namespace Core.Features.Reviews.Queries.Models
{
    public record GetReviewPaginatedListQuery(Guid ProductId, int PageNumber, int PageSize, string? Search,
        ReviewSortingEnum SortBy) : IRequest<ApiResponse<PaginatedResult<GetReviewPaginatedListResponse>>>;
}
