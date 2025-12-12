namespace Application.Features.Deliveries.Commands.SetDeliveryMethod;

public class SetDeliveryMethodValidator : AbstractValidator<SetDeliveryMethodCommand>
{
    public SetDeliveryMethodValidator()
    {
        ApplyValidationRules();
    }

    public void ApplyValidationRules()
    {
        RuleFor(c => c.OrderId)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required");
    }
}

