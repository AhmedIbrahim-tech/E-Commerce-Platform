using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Discounts.Commands.AddDiscount;

public class AddDiscountValidator : AbstractValidator<AddDiscountCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddDiscountValidator(IUnitOfWork unitOfWork)
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

        RuleFor(c => c.DiscountAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Discount amount must be greater than or equal to 0");

        RuleFor(c => c.EndDate)
            .GreaterThan(c => c.StartDate).WithMessage("End date must be after start date");
    }

    public void ApplyCustomValidationRules()
    {
        RuleFor(c => c.Name)
            .MustAsync(async (name, cancellation) => !await _unitOfWork.Discounts.GetTableNoTracking()
                .Where(d => d.Name.Equals(name))
                .AnyAsync(cancellation))
            .WithMessage("Already exists");
    }
}
