using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Categories.Commands.EditCategory;

public class EditCategoryValidator : AbstractValidator<EditCategoryCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public EditCategoryValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        ApplyValidationRules();
        ApplyCustomValidationRules();
    }

    public void ApplyValidationRules()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required");

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
            .MustAsync(async (model, name, cancellation) => !await _unitOfWork.Categories.GetTableNoTracking()
                .Where(c => c.Name!.Equals(name) && !c.Id.Equals(model.Id))
                .AnyAsync(cancellation))
            .WithMessage("Already exists");
    }
}

