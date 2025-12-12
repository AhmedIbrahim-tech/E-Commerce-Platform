using Application.Common.Bases;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetProductByIdQuery, ApiResponse<GetProductByIdResponse>>
{
    public async Task<ApiResponse<GetProductByIdResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.Products.GetTableNoTracking()
            .Where(c => c.Id.Equals(request.ProductId))
            .Include(c => c.Category)
            .FirstOrDefaultAsync(cancellationToken);

        if (product is null) return new ApiResponse<GetProductByIdResponse>(ProductErrors.ProductNotFound());

        var productResponse = new GetProductByIdResponse(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.StockQuantity,
            product.ImageURL,
            product.CreatedAt,
            product.Category?.Name
        );

        Expression<Func<Review, ReviewResponse>> expression = review => new ReviewResponse(
            review.CustomerId,
            review.Customer != null ? review.Customer.FullName : null,
            review.Rating,
            review.Comment
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

        var reviewPaginatedList = await queryable.Select(expression)
                                                        .ToPaginatedListAsync(request.ReviewPageNumber, request.ReviewPageSize);
        productResponse.Reviews = reviewPaginatedList;

        return Success(productResponse);
    }
}

