using Application.Common.Bases;

namespace Application.Features.Authorization.Queries.GetRoleList;

public record GetRoleListQuery : IRequest<ApiResponse<List<GetRoleListResponse>>>;

