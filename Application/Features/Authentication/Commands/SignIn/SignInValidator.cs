namespace Application.Features.Authentication.Commands.SignIn;

public class SignInValidator : AbstractValidator<SignInCommand>
{
    public SignInValidator()
    {
        ApplyValidationRoles();
    }

    public void ApplyValidationRoles()
    {
        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("Email cannot be empty")
            .NotNull().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(c => c.Password)
            .NotEmpty().WithMessage("Password cannot be empty")
            .NotNull().WithMessage("Password is required");
    }
}

