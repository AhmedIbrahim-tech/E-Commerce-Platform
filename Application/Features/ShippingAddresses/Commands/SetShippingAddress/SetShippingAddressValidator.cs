namespace Application.Features.ShippingAddresses.Commands.SetShippingAddress;

public class SetShippingAddressValidator : AbstractValidator<SetShippingAddressCommand>
{
    public SetShippingAddressValidator()
    {
        ApplyValidationRules();
    }

    public void ApplyValidationRules()
    {
        RuleFor(c => c.OrderId)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required");

        RuleFor(c => c.ShippingAddressId)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required");
    }
}

