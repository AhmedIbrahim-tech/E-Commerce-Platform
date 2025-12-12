namespace Application.Features.Carts.Commands.UpdateItemQuantity;

public class UpdateItemQuantityValidator : AbstractValidator<UpdateItemQuantityCommand>
{
    public UpdateItemQuantityValidator()
    {
        ApplyValidationRules();
    }

    public void ApplyValidationRules()
    {
        RuleFor(c => c.ProductId)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required");

        RuleFor(c => c.Quantity)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .GreaterThan(0).WithMessage("Must be greater than zero");
    }
}

