namespace Application.Features.ShippingAddresses.Commands.DeleteShippingAddress;

public class DeleteShippingAddressValidator : AbstractValidator<DeleteShippingAddressCommand>
{
    public DeleteShippingAddressValidator()
    {
        ApplyValidationRules();
    }

    public void ApplyValidationRules()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required");
    }
}

