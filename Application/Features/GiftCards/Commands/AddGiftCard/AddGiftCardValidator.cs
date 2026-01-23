using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.GiftCards.Commands.AddGiftCard;

public class AddGiftCardValidator : AbstractValidator<AddGiftCardCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddGiftCardValidator(IUnitOfWork unitOfWork)
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

        RuleFor(c => c.RecipientName)
            .MaximumLength(200).WithMessage("Maximum length is 200 characters");

        RuleFor(c => c.RecipientEmail)
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(256).WithMessage("Maximum length is 256 characters");

        RuleFor(c => c.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0");
    }

    public void ApplyCustomValidationRules()
    {
        RuleFor(c => c.Code)
            .MustAsync(async (code, cancellation) => !await _unitOfWork.GiftCards.GetTableNoTracking()
                .Where(gc => gc.Code.Equals(code))
                .AnyAsync(cancellation))
            .WithMessage("Code already exists");
    }
}
