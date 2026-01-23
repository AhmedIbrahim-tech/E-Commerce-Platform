using Application.Common.Bases;

namespace Application.Features.Admins.Queries.GetAdminById;

public record GetAdminByIdQuery(Guid Id) : IRequest<ApiResponse<GetAdminByIdResponse>>;
