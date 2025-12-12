namespace Application.Features.Authentication.ConfirmEmail;

public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailQuery>
{
    public ConfirmEmailValidator()
    {
        ApplyValidationRoles();
    }

    public void ApplyValidationRoles()
    {
        RuleFor(c => c.UserId)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required");

        RuleFor(c => c.Code)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required");
    }
}

