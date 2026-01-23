using Application.Common.Bases;

namespace Application.Features.ApplicationUser.Queries.GetUserById;

public record GetUserByIdQuery(Guid Id) : IRequest<ApiResponse<GetUserByIdResponse>>;
