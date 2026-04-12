namespace Application.Features.ApplicationUser.Commands.AddVendor;

public record AddVendorCommand : IRequest<ApiResponse<string>>
{
    public string? FullName { get; init; }
    public string? UserName { get; init; }
    public string? Email { get; init; }
    public Gender? Gender { get; init; }
    public string? PhoneNumber { get; init; }
    public string? SecondPhoneNumber { get; init; }
    public string? Password { get; init; }
    public string? ConfirmPassword { get; init; }
    public string StoreName { get; init; }
    public decimal CommissionRate { get; init; }
    public IFormFile? ProfileImage { get; init; }
    // Note: Role is ALWAYS Vendor (determined by server), NOT from request
    // Note: Claims are assigned based on creator permissions, NOT from request
    // Note: If created by Vendor owner, user is linked to same vendor/store
}
