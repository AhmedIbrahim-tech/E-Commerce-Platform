using Core.Features.Categories.Queries.Models;
using Core.Features.Categories.Queries.Response;

namespace Core.Features.Categories.Queries.Handlers
{
    public class CategoryQueryHandler : ApiResponseHandler
        , IRequestHandler<GetCategoryListQuery, ApiResponse<List<GetCategoryListResponse>>>
        , IRequestHandler<GetCategoryByIdQuery, ApiResponse<GetSingleCategoryResponse>>
        , IRequestHandler<GetCategoryPaginatedListQuery, PaginatedResult<GetCategoryPaginatedListResponse>>
    {
        #region Fields
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        public CategoryQueryHandler(ICategoryService categoryService,
            IMapper mapper) : base()
        {
            _categoryService = categoryService;
            _mapper = mapper;        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<List<GetCategoryListResponse>>> Handle(GetCategoryListQuery request, CancellationToken cancellationToken)
        {
            var categoryList = await _categoryService.GetCategoryListAsync();
            var categoryListMapper = _mapper.Map<List<GetCategoryListResponse>>(categoryList);
            return Success(categoryListMapper);
        }

        public async Task<ApiResponse<GetSingleCategoryResponse>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryService.GetCategoryByIdAsync(request.Id);
            if (category is null) return NotFound<GetSingleCategoryResponse>(SharedResourcesKeys.CategoryNotFound);
            var categoryMapper = _mapper.Map<GetSingleCategoryResponse>(category);
            return Success(categoryMapper);
        }

        public async Task<PaginatedResult<GetCategoryPaginatedListResponse>> Handle(GetCategoryPaginatedListQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Category, GetCategoryPaginatedListResponse>> expression = c => new GetCategoryPaginatedListResponse(c.Id, c.Name!, c.Description);
            var filterQuery = _categoryService.FilterCategoryPaginatedQueryable(request.SortBy, request.Search!);
            var paginatedList = await filterQuery.Select(expression!).ToPaginatedListAsync(request.PageNumber, request.PageSize);
            paginatedList.Meta = new { Count = paginatedList.Data.Count() };
            return paginatedList;
        }
        #endregion
    }
}
