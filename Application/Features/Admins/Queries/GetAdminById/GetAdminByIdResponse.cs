namespace Application.Features.Admins.Queries.GetAdminById;

public record GetAdminByIdResponse
{
    public Guid Id { get; init; }
    public Guid AppUserId { get; init; }
    public string? FullName { get; init; }
    public string? UserName { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public Gender? Gender { get; init; }
    public string? Address { get; init; }
    public string? ProfileImage { get; init; }
}
