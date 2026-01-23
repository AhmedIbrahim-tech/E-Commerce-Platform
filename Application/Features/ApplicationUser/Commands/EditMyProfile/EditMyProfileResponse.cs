namespace Application.Features.ApplicationUser.Commands.EditMyProfile;

public class EditMyProfileResponse
{
    public Guid Id { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public string? ProfileImageUrl { get; init; }
}

