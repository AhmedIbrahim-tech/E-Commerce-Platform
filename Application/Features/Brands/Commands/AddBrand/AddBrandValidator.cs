using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Brands.Commands.AddBrand;

public class AddBrandValidator : AbstractValidator<AddBrandCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddBrandValidator(IUnitOfWork unitOfWork)
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
    }

    public void ApplyCustomValidationRules()
    {
        RuleFor(c => c.Name)
            .MustAsync(async (name, cancellation) => !await _unitOfWork.Brands.GetTableNoTracking()
                .Where(b => b.Name.Equals(name))
                .AnyAsync(cancellation))
            .WithMessage("Already exists");
    }
}
