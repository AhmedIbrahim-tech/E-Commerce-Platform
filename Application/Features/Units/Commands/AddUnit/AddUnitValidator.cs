using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Units.Commands.AddUnit;

public class AddUnitValidator : AbstractValidator<AddUnitCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddUnitValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        ApplyValidationRules();
        ApplyCustomValidationRules();
    }

    public void ApplyValidationRules()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .MaximumLength(100).WithMessage("Maximum length is 100 characters");

        RuleFor(c => c.ShortName)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .MaximumLength(20).WithMessage("Maximum length is 20 characters");
            
        RuleFor(c => c.Description)
            .MaximumLength(300).WithMessage("Maximum length is 300 characters");
    }

    public void ApplyCustomValidationRules()
    {
        RuleFor(c => c.Name)
            .MustAsync(async (name, cancellation) => !await _unitOfWork.UnitOfMeasures.GetTableNoTracking()
                .Where(u => u.Name.Equals(name))
                .AnyAsync(cancellation))
            .WithMessage("Already exists");

        RuleFor(c => c.ShortName)
            .MustAsync(async (shortName, cancellation) => !await _unitOfWork.UnitOfMeasures.GetTableNoTracking()
                .Where(u => u.ShortName.Equals(shortName))
                .AnyAsync(cancellation))
            .WithMessage("Short name already exists");
    }
}
