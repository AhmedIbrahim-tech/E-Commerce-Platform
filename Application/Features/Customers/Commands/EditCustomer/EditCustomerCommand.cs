using Application.Common.Bases;

namespace Application.Features.Customers.Commands.EditCustomer;

public record EditCustomerCommand : IRequest<ApiResponse<string>>
{
    public Guid Id { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? UserName { get; init; }
    public string? Email { get; init; }
    public Gender? Gender { get; init; }
    public string? PhoneNumber { get; init; }
}

