using Application.Common.Bases;

namespace Application.Features.Categories.Queries.GetCategoryById;

public record GetCategoryByIdQuery(Guid Id) : IRequest<ApiResponse<GetCategoryByIdResponse>>;

