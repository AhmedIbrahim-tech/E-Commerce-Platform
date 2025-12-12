using Application.Common.Bases;

namespace Application.Features.Categories.Queries.GetCategoryList;

public record GetCategoryListQuery : IRequest<ApiResponse<List<GetCategoryListResponse>>>;

