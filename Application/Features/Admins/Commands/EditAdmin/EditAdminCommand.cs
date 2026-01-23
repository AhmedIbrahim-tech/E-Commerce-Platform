using Microsoft.AspNetCore.Http;

namespace Application.Features.Admins.Commands.EditAdmin;

public record EditAdminCommand : IRequest<ApiResponse<string>>
{
    public Guid Id { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? UserName { get; init; }
    public string? Email { get; init; }
    public Gender? Gender { get; init; }
    public string? PhoneNumber { get; init; }
    public string? SecondPhoneNumber { get; init; }
    public string? Address { get; init; }
    public IFormFile? ProfileImage { get; init; }
}
