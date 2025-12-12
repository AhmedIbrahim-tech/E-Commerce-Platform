using Application.Common.Bases;
using Domain.Responses;

namespace Application.Features.Authorization.Queries.ManageUserClaims;

public record ManageUserClaimsQuery(Guid UserId) : IRequest<ApiResponse<ManageUserClaimsResponse>>;

