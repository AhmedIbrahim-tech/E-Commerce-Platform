using FluentValidation;

namespace Application.Features.Vendors.Commands.EditVendor;

public class EditVendorValidator : AbstractValidator<EditVendorCommand>
{
    public EditVendorValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Vendor ID is required");

        RuleFor(v => v.FullName)
            .NotEmpty().WithMessage("Full name is required")
            .MaximumLength(150).WithMessage("Maximum length is 150 characters");

        RuleFor(v => v.UserName)
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters")
            .MaximumLength(50).WithMessage("Username cannot exceed 50 characters");

        RuleFor(v => v.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(v => v.StoreName)
            .NotEmpty().WithMessage("Store name is required")
            .MaximumLength(200).WithMessage("Store name cannot exceed 200 characters");

        RuleFor(v => v.CommissionRate)
            .InclusiveBetween(0.01m, 99.99m)
            .WithMessage("Commission rate must be between 0.01 and 99.99");
    }
}
