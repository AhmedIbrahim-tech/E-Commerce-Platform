using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Products.Commands.EditProduct;

public class EditProductValidator : AbstractValidator<EditProductCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public EditProductValidator(IUnitOfWork unitOfWork)
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

        RuleFor(c => c.Price)
            .NotEmpty().WithMessage("Field cannot be empty");

        RuleFor(c => c.StockQuantity)
            .NotEmpty().WithMessage("Field cannot be empty");

        RuleFor(c => c.CategoryId)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required");
    }

    public void ApplyCustomValidationRules()
    {
        RuleFor(c => c.Name)
            .MustAsync(async (model, name, cancellation) => !await _unitOfWork.Products.GetTableNoTracking()
                .Where(c => c.Name!.Equals(name) && !c.Id.Equals(model.Id))
                .AnyAsync(cancellation))
            .WithMessage("Already exists");

        RuleFor(c => c.CategoryId)
            .MustAsync(async (key, cancellation) => await _unitOfWork.Categories.GetTableNoTracking()
                .AnyAsync(c => c.Id.Equals(key), cancellation))
            .WithMessage("Does not exist");
    }
}

