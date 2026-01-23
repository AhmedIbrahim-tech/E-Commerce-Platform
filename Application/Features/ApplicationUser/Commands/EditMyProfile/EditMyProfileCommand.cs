using Application.Common.Bases;

namespace Application.Features.ApplicationUser.Commands.EditMyProfile;

public record EditMyProfileCommand : IRequest<ApiResponse<EditMyProfileResponse>>
{
    public string? DisplayName { get; init; }
    public string? PhoneNumber { get; init; }
    public IFormFile? ProfileImage { get; init; }
}

