namespace Application.Features.Vendors.Queries.GetVendorById;

public record GetVendorByIdResponse
{
    public Guid Id { get; init; }
    public string? FullName { get; init; }
    public string? UserName { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public Gender? Gender { get; init; }
    public string StoreName { get; init; }
    public decimal CommissionRate { get; init; }
    public string? ProfileImage { get; init; }
}
