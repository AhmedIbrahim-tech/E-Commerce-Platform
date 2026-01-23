namespace Application.Features.ApplicationUser.Commands.AddAdmin;

public class AddAdminValidator : AbstractValidator<AddAdminCommand>
{
    public AddAdminValidator()
    {
        ApplyValidationRules();
    }

    public void ApplyValidationRules()
    {
        RuleFor(c => c.FirstName)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .MaximumLength(100).WithMessage("Maximum length is 100 characters");
        
        RuleFor(c => c.LastName)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .MaximumLength(50).WithMessage("Maximum length is 50 characters");
        
        RuleFor(c => c.UserName)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .MaximumLength(50).WithMessage("Maximum length is 50 characters");
        
        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .EmailAddress().WithMessage("Invalid format");
        
        RuleFor(c => c.Password)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .MinimumLength(6).WithMessage("Minimum length is 6 characters");
        
        RuleFor(c => c.ConfirmPassword)
            .Equal(c => c.Password).WithMessage("Passwords do not match");
        
        RuleFor(c => c.Address)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .MaximumLength(250).WithMessage("Maximum length is 250 characters");
    }
}
