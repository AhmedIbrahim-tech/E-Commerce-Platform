namespace Domain.Entities;

public class Admin
{
    public Guid Id { get; set; }
    public string? FullName { get; set; }
    public Gender? Gender { get; set; }
    public string? Address { get; set; }
    public Guid AppUserId { get; set; }
}
