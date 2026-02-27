namespace Application.Features.ApplicationUser.Queries.GetUserById;

public record GetUserByIdResponse
{
    public Guid Id { get; init; }
    public Guid AppUserId { get; init; }
    public string? FullName { get; init; }
    public string? UserName { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public string? SecondPhoneNumber { get; init; }
    public Gender? Gender { get; init; }
    public string? Address { get; init; }
    public string? Role { get; init; }
    public List<string> Roles { get; init; } = new();
    public string? ProfileImage { get; init; }
    public bool IsActive { get; init; }
    public DateTimeOffset? CreatedAt { get; init; }
    public DateTimeOffset? LastLoginAt { get; init; }
    public List<string> Claims { get; init; } = new();
}
