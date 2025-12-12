using Application.Common.Bases;
using Domain.Responses;

namespace Application.Features.Authorization.Queries.ManageUserRoles;

public record ManageUserRolesQuery(Guid UserId) : IRequest<ApiResponse<ManageUserRolesResponse>>;

