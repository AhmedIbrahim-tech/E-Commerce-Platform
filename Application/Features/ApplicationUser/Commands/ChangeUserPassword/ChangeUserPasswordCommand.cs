using Application.Common.Bases;

namespace Application.Features.ApplicationUser.Commands.ChangeUserPassword
{
    public record ChangeUserPasswordCommand : IRequest<ApiResponse<string>>
    {
        public Guid Id { get; init; }
        public string CurrentPassword { get; init; }
        public string NewPassword { get; init; }
        public string ConfirmPassword { get; init; }
    }
}

