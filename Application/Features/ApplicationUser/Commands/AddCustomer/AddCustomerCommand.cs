
using Application.Common.Bases;

namespace Application.Features.ApplicationUser.Commands.AddCustomer
{
    public record AddCustomerCommand : IRequest<ApiResponse<string>>
    {
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? UserName { get; init; }
        public string? Email { get; init; }
        public Gender? Gender { get; init; }
        public string? PhoneNumber { get; init; }
        public string? Password { get; init; }
        public string? ConfirmPassword { get; init; }
    }
}

