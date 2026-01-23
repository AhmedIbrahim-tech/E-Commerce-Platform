using Application.Common.Bases;

namespace Application.Features.Units.Queries.GetUnitList;

public record GetUnitListQuery() : IRequest<ApiResponse<List<GetUnitListResponse>>>;
