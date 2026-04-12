namespace Application.Features.ApplicationUser.Commands.AddVendor;

public class AddVendorValidator : AbstractValidator<AddVendorCommand>
{
    public AddVendorValidator()
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
        
        RuleFor(c => c.Password)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .MinimumLength(6).WithMessage("Minimum length is 6 characters");
        
        RuleFor(c => c.ConfirmPassword)
            .Equal(c => c.Password).WithMessage("Passwords do not match");
        
        // StoreName and CommissionRate are required only when created by Admin/SuperAdmin
        // When created by Vendor owner, they're copied from creator's vendor
        // Validation is handled in the handler based on creator role
    }
}
