using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Categories.Commands.AddCategory;

public class AddCategoryValidator : AbstractValidator<AddCategoryCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddCategoryValidator(IUnitOfWork unitOfWork)
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
            
        RuleFor(c => c.Description)
            .MaximumLength(300).WithMessage("Maximum length is 300 characters");
    }

    public void ApplyCustomValidationRules()
    {
        RuleFor(c => c.Name)
            .MustAsync(async (name, cancellation) => !await _unitOfWork.Categories.GetTableNoTracking()
                .Where(c => c.Name!.Equals(name))
                .AnyAsync(cancellation))
            .WithMessage("Already exists");
    }
}

