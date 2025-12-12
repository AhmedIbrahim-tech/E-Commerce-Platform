using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.ShippingAddresses.Commands.AddShippingAddress;

public class AddShippingAddressValidator : AbstractValidator<AddShippingAddressCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddShippingAddressValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        ApplyValidationRules();
        ApplyCustomValidationRules();
    }

    public void ApplyValidationRules()
    {
        RuleFor(c => c.FirstName)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .MaximumLength(100).WithMessage("Maximum length is 100 characters");

        RuleFor(c => c.LastName)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .MaximumLength(100).WithMessage("Maximum length is 100 characters");

        RuleFor(c => c.Street)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .MaximumLength(100).WithMessage("Maximum length is 100 characters");

        RuleFor(c => c.City)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .MaximumLength(100).WithMessage("Maximum length is 100 characters");

        RuleFor(c => c.State)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .MaximumLength(100).WithMessage("Maximum length is 100 characters");
    }

    public void ApplyCustomValidationRules()
    {
        RuleFor(c => c)
            .MustAsync(async (shippingAddress, cancellation) => !await _unitOfWork.ShippingAddresses.GetTableNoTracking()
                .AnyAsync(c => c.Street == shippingAddress.Street && c.City == shippingAddress.City && c.State == shippingAddress.State, cancellation))
            .WithMessage("Shipping address already exists");
    }
}

