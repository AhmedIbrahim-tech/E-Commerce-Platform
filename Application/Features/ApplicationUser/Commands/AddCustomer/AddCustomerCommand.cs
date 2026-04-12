namespace Application.Features.ApplicationUser.Commands.AddCustomer;

public record AddCustomerCommand : IRequest<ApiResponse<string>>
{
    public string? FullName { get; init; }
    public string? UserName { get; init; }
    public string? Email { get; init; }
    public Gender? Gender { get; init; }
    public string? PhoneNumber { get; init; }
    public string? SecondPhoneNumber { get; init; }
    public string? Password { get; init; }
    public string? ConfirmPassword { get; init; }
    /// <summary>Optional profile / avatar image (same storage as admin & vendor).</summary>
    public IFormFile? ProfileImage { get; init; }
}

