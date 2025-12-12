namespace Application.Features.ApplicationUser.Commands.ChangeUserPassword
{
    public class ChangeUserPasswordValidator : AbstractValidator<ChangeUserPasswordCommand>
    {
        public ChangeUserPasswordValidator()
        {
            ApplyValidationRoles();
        }

        public void ApplyValidationRoles()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Field cannot be empty")
                .NotNull().WithMessage("Field is required");

            RuleFor(c => c.CurrentPassword)
                .NotEmpty().WithMessage("Field cannot be empty")
                .NotNull().WithMessage("Field is required");

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
}

