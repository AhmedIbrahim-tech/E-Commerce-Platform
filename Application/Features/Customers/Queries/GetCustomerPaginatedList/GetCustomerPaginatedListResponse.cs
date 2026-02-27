namespace Application.Features.Customers.Queries.GetCustomerPaginatedList;

public record GetCustomerPaginatedListResponse
{
    public Guid Id { get; init; }
    public string? FullName { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public Gender? Gender { get; init; }
    public string? Role { get; init; }
    public string? ProfileImage { get; init; }
    public bool IsDeleted { get; init; }
}

