using Application.Common.Bases;

namespace Application.Features.Authorization.Queries.GetRoleById;

public record GetRoleByIdQuery(Guid Id) : IRequest<ApiResponse<GetRoleByIdResponse>>;

