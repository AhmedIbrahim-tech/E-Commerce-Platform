namespace Application.Features.Vendors.Queries.GetVendorPaginatedList;

public record GetVendorPaginatedListResponse
{
    public Guid Id { get; init; }
    public string? FullName { get; init; }
    public string? UserName { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public Gender? Gender { get; init; }
    public string StoreName { get; init; }
    public decimal CommissionRate { get; init; }
    public string? Role { get; init; }
    public string? ProfileImage { get; init; }
    public bool IsDeleted { get; init; }
}
