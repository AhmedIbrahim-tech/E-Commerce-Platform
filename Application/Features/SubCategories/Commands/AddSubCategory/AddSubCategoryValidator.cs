using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.SubCategories.Commands.AddSubCategory;

public class AddSubCategoryValidator : AbstractValidator<AddSubCategoryCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddSubCategoryValidator(IUnitOfWork unitOfWork)
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

        RuleFor(c => c.ImageUrl)
            .MaximumLength(500).WithMessage("Maximum length is 500 characters");

        RuleFor(c => c.Code)
            .MaximumLength(50).WithMessage("Maximum length is 50 characters");

        RuleFor(c => c.CategoryId)
            .NotEmpty().WithMessage("Category is required");
    }

    public void ApplyCustomValidationRules()
    {
        RuleFor(c => c.Name)
            .MustAsync(async (command, name, cancellation) => !await _unitOfWork.SubCategories.GetTableNoTracking()
                .Where(sc => sc.Name.Equals(name) && sc.CategoryId == command.CategoryId)
                .AnyAsync(cancellation))
            .WithMessage("Already exists");

        RuleFor(c => c.CategoryId)
            .MustAsync(async (categoryId, cancellation) => await _unitOfWork.Categories.GetTableNoTracking()
                .AnyAsync(c => c.Id == categoryId, cancellation))
            .WithMessage("Category not found");
    }
}
