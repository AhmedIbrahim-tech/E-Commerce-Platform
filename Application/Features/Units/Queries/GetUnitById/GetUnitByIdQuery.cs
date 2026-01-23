using Application.Common.Bases;

namespace Application.Features.Units.Queries.GetUnitById;

public record GetUnitByIdQuery(Guid Id) : IRequest<ApiResponse<GetUnitByIdResponse>>;
