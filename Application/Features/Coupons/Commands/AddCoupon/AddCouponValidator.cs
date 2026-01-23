using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Coupons.Commands.AddCoupon;

public class AddCouponValidator : AbstractValidator<AddCouponCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddCouponValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        ApplyValidationRules();
        ApplyCustomValidationRules();
    }

    public void ApplyValidationRules()
    {
        RuleFor(c => c.Code)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .MaximumLength(50).WithMessage("Maximum length is 50 characters");

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
        RuleFor(c => c.Code)
            .MustAsync(async (code, cancellation) => !await _unitOfWork.Coupons.GetTableNoTracking()
                .Where(c => c.Code.Equals(code))
                .AnyAsync(cancellation))
            .WithMessage("Code already exists");
    }
}
