namespace Application.Features.Admins.Commands.EditAdmin;

public class EditAdminValidator : AbstractValidator<EditAdminCommand>
{
    public EditAdminValidator()
    {
        ApplyValidationRules();
    }

    public void ApplyValidationRules()
    {
        RuleFor(c => c.FullName)
            .NotEmpty().WithMessage("Full name is required")
            .MaximumLength(150).WithMessage("Maximum length is 150 characters");

        RuleFor(c => c.UserName)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .MaximumLength(50).WithMessage("Maximum length is 50 characters");

        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .EmailAddress().WithMessage("Invalid format");
    }
}
