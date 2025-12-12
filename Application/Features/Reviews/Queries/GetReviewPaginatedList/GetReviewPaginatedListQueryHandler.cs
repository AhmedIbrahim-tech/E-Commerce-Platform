using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Reviews.Queries.GetReviewPaginatedList;

public class GetReviewPaginatedListQueryHandler(
    IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetReviewPaginatedListQuery, ApiResponse<PaginatedResult<GetReviewPaginatedListResponse>>>
{
    public async Task<ApiResponse<PaginatedResult<GetReviewPaginatedListResponse>>> Handle(GetReviewPaginatedListQuery request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.Products.GetTableNoTracking()
            .Where(c => c.Id.Equals(request.ProductId))
            .Include(c => c.Category)
            .FirstOrDefaultAsync(cancellationToken);

        if (product is null) return new ApiResponse<PaginatedResult<GetReviewPaginatedListResponse>>(ProductErrors.ProductNotFound());

        Expression<Func<Review, GetReviewPaginatedListResponse>> expression = c => new GetReviewPaginatedListResponse(
            c.Customer!.FullName ?? string.Empty,
            c.Product!.Name,
            c.Rating,
            c.Comment,
            c.CreatedAt
        );

        var queryable = unitOfWork.Reviews.GetTableNoTracking()
            .Where(r => r.ProductId == request.ProductId)
            .Include(r => r.Customer)
            .Include(r => r.Product);

        if (!string.IsNullOrWhiteSpace(request.Search))
            queryable = queryable.Where(c => c.Comment!.Contains(request.Search!));

        queryable = request.SortBy switch
        {
            ReviewSortingEnum.CreatedDateAsc => queryable.OrderBy(c => c.CreatedAt),
            ReviewSortingEnum.CreatedDateDesc => queryable.OrderByDescending(c => c.CreatedAt),
            ReviewSortingEnum.RatingAsc => queryable.OrderBy(c => c.Rating),
            ReviewSortingEnum.RatingDesc => queryable.OrderByDescending(c => c.Rating),
            _ => queryable.OrderByDescending(c => c.CreatedAt)
                          .ThenByDescending(c => c.Rating)
        };

        var paginatedList = await queryable.Select(expression)
                                             .ToPaginatedListAsync(request.PageNumber, request.PageSize);
        return Success(paginatedList);
    }
}

