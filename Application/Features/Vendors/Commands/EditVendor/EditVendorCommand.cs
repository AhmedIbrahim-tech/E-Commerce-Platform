using Application.Common.Bases;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Vendors.Commands.EditVendor;

public record EditVendorCommand : IRequest<ApiResponse<string>>
{
    public Guid Id { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? UserName { get; init; }
    public string? Email { get; init; }
    public Gender? Gender { get; init; }
    public string? PhoneNumber { get; init; }
    public string? SecondPhoneNumber { get; init; }
    public string StoreName { get; init; }
    public decimal CommissionRate { get; init; }
    public IFormFile? ProfileImage { get; init; }
}
