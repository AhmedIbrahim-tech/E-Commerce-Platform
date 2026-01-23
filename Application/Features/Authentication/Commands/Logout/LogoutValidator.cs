namespace Application.Features.Authentication.Commands.Logout;

public class LogoutValidator : AbstractValidator<LogoutCommand>
{
    public LogoutValidator()
    {
        RuleFor(c => c.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required")
            .NotNull().WithMessage("Refresh token cannot be null");
    }
}
