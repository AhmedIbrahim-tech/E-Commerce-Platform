namespace Application.Features.Authentication.Commands.RefreshToken;

public class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenValidator()
    {
        ApplyValidationRoles();
    }

    public void ApplyValidationRoles()
    {
        RuleFor(c => c.RefreshToken)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required");

        When(c => !string.IsNullOrWhiteSpace(c.AccessToken), () =>
        {
            RuleFor(c => c.AccessToken!)
                .MinimumLength(20).WithMessage("Invalid access token");
        });
    }
}

