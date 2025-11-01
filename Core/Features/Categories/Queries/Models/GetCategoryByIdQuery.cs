using Core.Features.Categories.Queries.Response;

namespace Core.Features.Categories.Queries.Models
{
    public record GetCategoryByIdQuery(Guid Id) : IRequest<ApiResponse<GetSingleCategoryResponse>>;
}
