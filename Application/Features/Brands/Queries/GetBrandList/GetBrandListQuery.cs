using Application.Common.Bases;

namespace Application.Features.Brands.Queries.GetBrandList;

public record GetBrandListQuery() : IRequest<ApiResponse<List<GetBrandListResponse>>>;
