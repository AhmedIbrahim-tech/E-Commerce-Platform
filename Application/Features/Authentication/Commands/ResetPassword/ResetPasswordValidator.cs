namespace Application.Features.Authentication.ResetPassword;

public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordValidator()
    {
        ApplyValidationRoles();
    }

    public void ApplyValidationRoles()
    {
        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .EmailAddress().WithMessage("Invalid format");

        RuleFor(c => c.NewPassword)
                .NotEmpty().WithMessage("Field cannot be empty")
                .MinimumLength(6).WithMessage("Minimum length is 6 characters")
                .NotNull().WithMessage("Field is required");

        RuleFor(c => c.ConfirmPassword)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .Equal(c => c.NewPassword).WithMessage("Passwords do not match");
    }
}

