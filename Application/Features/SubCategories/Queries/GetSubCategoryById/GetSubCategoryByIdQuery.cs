using Application.Common.Bases;

namespace Application.Features.SubCategories.Queries.GetSubCategoryById;

public record GetSubCategoryByIdQuery(Guid Id) : IRequest<ApiResponse<GetSubCategoryByIdResponse>>;
