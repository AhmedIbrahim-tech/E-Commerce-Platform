using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.ShippingAddresses.Commands.EditShippingAddress;

public class EditShippingAddressValidator : AbstractValidator<EditShippingAddressCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public EditShippingAddressValidator(IUnitOfWork unitOfWork)
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
            .MustAsync(async (model, cancellation) => 
            {
                var existingAddress = await _unitOfWork.ShippingAddresses.GetTableNoTracking()
                    .Where(c => c.Street == model.Street && c.City == model.City && c.State == model.State && !c.Id.Equals(model.Id))
                    .FirstOrDefaultAsync(cancellation);
                return existingAddress == null;
            })
            .WithMessage("Shipping address already exists");
    }
}

