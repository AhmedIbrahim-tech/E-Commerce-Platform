using Core.Features.Reviews.Queries.Models;
using Core.Features.Reviews.Queries.Responses;

namespace Core.Features.Reviews.Queries.Handlers
{
    public class ReviewQueryHandler : ApiResponseHandler,
        IRequestHandler<GetReviewPaginatedListQuery, ApiResponse<PaginatedResult<GetReviewPaginatedListResponse>>>
    {
        #region Fields
        private readonly IReviewService _reviewService;
        private readonly IProductService _productService;
        #endregion

        #region Constructors
        public ReviewQueryHandler(IReviewService reviewService,
            IProductService productService) : base()
        {
            _reviewService = reviewService;
            _productService = productService;        }
        #endregion

        #region Functions Handle
        public async Task<ApiResponse<PaginatedResult<GetReviewPaginatedListResponse>>> Handle(GetReviewPaginatedListQuery request, CancellationToken cancellationToken)
        {
            var product = await _productService.GetProductByIdAsync(request.ProductId);
            if (product is null) return BadRequest<PaginatedResult<GetReviewPaginatedListResponse>>(SharedResourcesKeys.ProductNotFound);

            Expression<Func<Review, GetReviewPaginatedListResponse>> expression = c => new GetReviewPaginatedListResponse
            (
                c.Customer!.FirstName + " " + c.Customer.LastName,
                c.Product!.Name,
                c.Rating,
                c.Comment,
                c.CreatedAt
            );

            var filterQuery = _reviewService.FilterReviewPaginatedQueryable(request.SortBy, request.Search!, request.ProductId);
            var paginatedList = await filterQuery.Select(expression!)
                                                 .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return Success(paginatedList);
        }
        #endregion
    }
}
