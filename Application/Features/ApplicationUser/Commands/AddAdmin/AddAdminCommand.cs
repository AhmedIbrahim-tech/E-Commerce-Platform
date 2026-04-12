namespace Application.Features.ApplicationUser.Commands.AddAdmin;

public record AddAdminCommand : IRequest<ApiResponse<string>>
{
    public string? FullName { get; init; }
    public string? UserName { get; init; }
    public string? Email { get; init; }
    public Gender? Gender { get; init; }
    public string? PhoneNumber { get; init; }
    public string? SecondPhoneNumber { get; init; }
    public string? Password { get; init; }
    public string? ConfirmPassword { get; init; }
    public string? Address { get; init; }
    public IFormFile? ProfileImage { get; init; }
    // Note: Role is determined by server based on creator, NOT from request
    // Note: Claims are assigned based on creator permissions, NOT from request
}
