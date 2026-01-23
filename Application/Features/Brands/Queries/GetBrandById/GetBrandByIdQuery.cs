using Application.Common.Bases;

namespace Application.Features.Brands.Queries.GetBrandById;

public record GetBrandByIdQuery(Guid Id) : IRequest<ApiResponse<GetBrandByIdResponse>>;
