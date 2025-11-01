using Core.Features.Categories.Queries.Response;

namespace Core.Features.Categories.Queries.Models
{
    public record GetCategoryListQuery : IRequest<ApiResponse<List<GetCategoryListResponse>>>;
}
