using Application.Common.Bases;

namespace Application.Features.ApplicationUser.Queries.GetMyProfile;

public record GetMyProfileQuery : IRequest<ApiResponse<GetMyProfileResponse>>;

