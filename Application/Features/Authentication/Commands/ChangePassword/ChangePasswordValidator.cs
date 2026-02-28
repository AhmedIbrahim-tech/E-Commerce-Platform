namespace Application.Features.Authentication.Commands.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordValidator()
    {
        RuleFor(c => c.CurrentPassword)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required");

        RuleFor(c => c.NewPassword)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .MinimumLength(6).WithMessage("Minimum length is 6 characters");

        RuleFor(c => c.ConfirmPassword)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .Equal(c => c.NewPassword).WithMessage("Passwords do not match");
    }
}
