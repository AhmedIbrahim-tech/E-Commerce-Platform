using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Accounts.Commands.AddAccount;

public class AddAccountValidator : AbstractValidator<AddAccountCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddAccountValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        ApplyValidationRules();
        ApplyCustomValidationRules();
    }

    public void ApplyValidationRules()
    {
        RuleFor(c => c.AccountName)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .MaximumLength(200).WithMessage("Maximum length is 200 characters");

        RuleFor(c => c.AccountNumber)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .MaximumLength(50).WithMessage("Maximum length is 50 characters");

        RuleFor(c => c.BankName)
            .MaximumLength(200).WithMessage("Maximum length is 200 characters");

        RuleFor(c => c.BranchName)
            .MaximumLength(200).WithMessage("Maximum length is 200 characters");

        RuleFor(c => c.Iban)
            .MaximumLength(50).WithMessage("Maximum length is 50 characters");

        RuleFor(c => c.SwiftCode)
            .MaximumLength(20).WithMessage("Maximum length is 20 characters");

        RuleFor(c => c.Description)
            .MaximumLength(300).WithMessage("Maximum length is 300 characters");
    }

    public void ApplyCustomValidationRules()
    {
        RuleFor(c => c.AccountNumber)
            .MustAsync(async (accountNumber, cancellation) => !await _unitOfWork.Accounts.GetTableNoTracking()
                .Where(a => a.AccountNumber.Equals(accountNumber))
                .AnyAsync(cancellation))
            .WithMessage("Account number already exists");
    }
}
