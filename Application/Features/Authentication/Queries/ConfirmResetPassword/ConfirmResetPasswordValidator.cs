namespace Application.Features.Authentication.ConfirmResetPassword;

public class ConfirmResetPasswordValidator : AbstractValidator<ConfirmResetPasswordQuery>
{
    public ConfirmResetPasswordValidator()
    {
        ApplyValidationRoles();
    }

    public void ApplyValidationRoles()
    {
        RuleFor(c => c.Code)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required");

        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required");
    }
}

