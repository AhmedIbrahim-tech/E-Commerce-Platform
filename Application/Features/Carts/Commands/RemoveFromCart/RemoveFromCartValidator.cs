namespace Application.Features.Carts.Commands.RemoveFromCart;

public class RemoveFromCartValidator : AbstractValidator<RemoveFromCartCommand>
{
    public RemoveFromCartValidator()
    {
        ApplyValidationRules();
    }

    public void ApplyValidationRules()
    {
        RuleFor(c => c.ProductId)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required");
    }
}

