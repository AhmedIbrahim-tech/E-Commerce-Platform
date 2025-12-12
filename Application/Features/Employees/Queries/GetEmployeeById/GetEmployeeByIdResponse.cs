namespace Application.Features.Employees.Queries.GetEmployeeById;

public record GetEmployeeByIdResponse
{
    public Guid Id { get; init; }
    public string? FullName { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public Gender? Gender { get; init; }
    public string? Position { get; init; }
    public decimal? Salary { get; init; }
    public DateTimeOffset? HireDate { get; init; }
    public string? Address { get; init; }
}

