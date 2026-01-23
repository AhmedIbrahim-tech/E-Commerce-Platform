namespace Application.Features.Admins.Queries.GetAdminPaginatedList;

public record GetAdminPaginatedListResponse
{
    public Guid Id { get; init; }
    public Guid AppUserId { get; init; }
    public string? FullName { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public Gender? Gender { get; init; }
    public string? Address { get; init; }
    public string? Role { get; init; }
    public string? ProfileImage { get; init; }
    public bool IsDeleted { get; init; }
}
