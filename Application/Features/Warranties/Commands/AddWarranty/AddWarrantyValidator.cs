using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Warranties.Commands.AddWarranty;

public class AddWarrantyValidator : AbstractValidator<AddWarrantyCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddWarrantyValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        ApplyValidationRules();
        ApplyCustomValidationRules();
    }

    public void ApplyValidationRules()
    {
        RuleFor(w => w.Name)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .MaximumLength(200).WithMessage("Maximum length is 200 characters");

        RuleFor(w => w.Description)
            .MaximumLength(500).WithMessage("Maximum length is 500 characters");

        RuleFor(w => w.Duration)
            .GreaterThan(0).WithMessage("Duration must be greater than 0");

        RuleFor(w => w.DurationPeriod)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .MaximumLength(20).WithMessage("Maximum length is 20 characters")
            .Must(p => p == "Month" || p == "Year" || p == "Months" || p == "Years")
            .WithMessage("Duration period must be Month, Months, Year, or Years");
    }

    public void ApplyCustomValidationRules()
    {
        RuleFor(w => w.Name)
            .MustAsync(async (name, cancellation) => !await _unitOfWork.Warranties.GetTableNoTracking()
                .Where(w => w.Name.Equals(name))
                .AnyAsync(cancellation))
            .WithMessage("Warranty with this name already exists");
    }
}
