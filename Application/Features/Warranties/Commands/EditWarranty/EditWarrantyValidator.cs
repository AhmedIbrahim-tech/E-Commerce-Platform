using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Warranties.Commands.EditWarranty;

public class EditWarrantyValidator : AbstractValidator<EditWarrantyCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public EditWarrantyValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        ApplyValidationRules();
        ApplyCustomValidationRules();
    }

    public void ApplyValidationRules()
    {
        RuleFor(w => w.Id)
            .NotEmpty().WithMessage("Id is required");

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
            .MustAsync(async (command, name, cancellation) => !await _unitOfWork.Warranties.GetTableNoTracking()
                .Where(w => w.Name.Equals(name) && w.Id != command.Id)
                .AnyAsync(cancellation))
            .WithMessage("Warranty with this name already exists");

        RuleFor(w => w.Id)
            .MustAsync(async (id, cancellation) => await _unitOfWork.Warranties.GetTableNoTracking()
                .AnyAsync(w => w.Id == id, cancellation))
            .WithMessage("Warranty not found");
    }
}
