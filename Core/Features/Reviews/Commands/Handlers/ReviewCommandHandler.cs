using Core.Features.Reviews.Commands.Models;

namespace Core.Features.Reviews.Commands.Handlers
{
    public class ReviewCommandHandler : ApiResponseHandler,
        IRequestHandler<AddReviewCommand, ApiResponse<string>>,
        IRequestHandler<EditReviewCommand, ApiResponse<string>>,
        IRequestHandler<DeleteReviewCommand, ApiResponse<string>>
    {
        #region Fields
        private readonly IReviewService _reviewService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        public ReviewCommandHandler(
            IReviewService reviewService,
            ICurrentUserService currentUserService,
            IMapper mapper) : base()
        {
            _reviewService = reviewService;
            _currentUserService = currentUserService;
            _mapper = mapper;        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<string>> Handle(AddReviewCommand request, CancellationToken cancellationToken)
        {
            var reviewMapper = _mapper.Map<Review>(request);
            var currentCustomerId = _currentUserService.GetUserId();
            reviewMapper.CustomerId = currentCustomerId;
            reviewMapper.CreatedAt = DateTimeOffset.UtcNow.ToLocalTime();
            var result = await _reviewService.AddReviewAsync(reviewMapper);
            if (result == "Success") return Created("");
            else return BadRequest<string>(SharedResourcesKeys.CreateFailed);
        }

        public async Task<ApiResponse<string>> Handle(EditReviewCommand request, CancellationToken cancellationToken)
        {
            var currentCustomerId = _currentUserService.GetUserId();
            var review = await _reviewService.GetReviewByIdsAsync(request.ProductId, currentCustomerId);
            if (review == null) return NotFound<string>(SharedResourcesKeys.ReviewNotFound);
            var reviewMapper = _mapper.Map(request, review);
            var result = await _reviewService.EditReviewAsync(reviewMapper);
            if (result == "Success") return Edit("");
            else return BadRequest<string>(SharedResourcesKeys.UpdateFailed);
        }

        public async Task<ApiResponse<string>> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var currentCustomerId = _currentUserService.GetUserId();
            var review = await _reviewService.GetReviewByIdsAsync(request.ProductId, currentCustomerId);
            if (review == null) return NotFound<string>(SharedResourcesKeys.ReviewNotFound);
            var result = await _reviewService.DeleteReviewAsync(review);
            if (result == "Success") return Deleted<string>();
            else return BadRequest<string>(SharedResourcesKeys.DeleteFailed);
        }
        #endregion
    }
}
