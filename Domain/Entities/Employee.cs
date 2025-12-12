namespace Domain.Entities;

public class Employee
{
    public Guid Id { get; set; }
    public string? FullName { get; set; }
    public Gender? Gender { get; set; }
    public string? Position { get; set; }
    public decimal? Salary { get; set; }
    public DateTimeOffset? HireDate { get; set; }
    public string? Address { get; set; }
    public Guid AppUserId { get; set; }
}
