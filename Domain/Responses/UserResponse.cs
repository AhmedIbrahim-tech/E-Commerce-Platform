namespace Domain.Responses;

public class User
{
    public Guid Id { get; set; }
    public Guid AppUserId { get; set; }
    public string? FullName { get; set; }
    public Gender? Gender { get; set; }
}
