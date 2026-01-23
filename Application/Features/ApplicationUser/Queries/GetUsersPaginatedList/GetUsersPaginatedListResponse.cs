namespace Application.Features.ApplicationUser.Queries.GetUsersPaginatedList;

public record GetUsersPaginatedListResponse
{
    public Guid Id { get; init; }
    public Guid AppUserId { get; init; }
    public string? FullName { get; init; }
    public string? UserName { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Role { get; init; }
    public string? ProfileImageUrl { get; init; }
    public bool IsActive { get; init; }
    public DateTimeOffset? CreatedAt { get; init; }
}
