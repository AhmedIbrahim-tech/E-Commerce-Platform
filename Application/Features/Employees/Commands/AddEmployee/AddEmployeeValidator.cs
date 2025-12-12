namespace Application.Features.Employees.Commands.AddEmployee;

public class AddEmployeeValidator : AbstractValidator<AddEmployeeCommand>
{
    public AddEmployeeValidator()
    {
        ApplyValidationRules();
    }

    public void ApplyValidationRules()
    {
        RuleFor(e => e.FirstName)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .MaximumLength(100).WithMessage("Maximum length is 100 characters");

        RuleFor(e => e.LastName)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .MaximumLength(50).WithMessage("Maximum length is 100 characters");

        RuleFor(e => e.UserName)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .MaximumLength(50).WithMessage("Maximum length is 100 characters");

        RuleFor(e => e.Email)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .EmailAddress().WithMessage("Invalid format");

        RuleFor(e => e.Password)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .MinimumLength(6).WithMessage("Minimum length is 6 characters");

        RuleFor(e => e.ConfirmPassword)
            .Equal(e => e.Password).WithMessage("Passwords do not match");

        RuleFor(e => e.Position)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .MaximumLength(100).WithMessage("Maximum length is 100 characters");

        RuleFor(e => e.Salary)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .GreaterThan(0).WithMessage("Must be greater than zero");

        RuleFor(e => e.Address)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .MaximumLength(200).WithMessage("Maximum length is 200 characters");
    }
}

