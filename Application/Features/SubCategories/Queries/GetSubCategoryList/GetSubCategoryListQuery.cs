using Application.Common.Bases;

namespace Application.Features.SubCategories.Queries.GetSubCategoryList;

public record GetSubCategoryListQuery(Guid? CategoryId = null) : IRequest<ApiResponse<List<GetSubCategoryListResponse>>>;
