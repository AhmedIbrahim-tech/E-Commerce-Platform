namespace Application.Features.Deliveries.Commands.EditDeliveryMethod;

public class EditDeliveryMethodValidator : AbstractValidator<EditDeliveryMethodCommand>
{
    public EditDeliveryMethodValidator()
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

