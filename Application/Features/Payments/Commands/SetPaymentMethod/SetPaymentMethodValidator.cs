using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Payments.Commands.SetPaymentMethod;

public class SetPaymentMethodValidator : AbstractValidator<SetPaymentMethodCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public SetPaymentMethodValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        ApplyValidationRules();
    }

    public void ApplyValidationRules()
    {
        RuleFor(c => c.OrderId)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required");

        RuleFor(c => c.PaymentMethod)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required");

        RuleFor(c => c).CustomAsync(async (command, context, cancellation) =>
        {
            var order = await _unitOfWork.Orders.GetTableNoTracking()
                .Where(c => c.Id.Equals(command.OrderId))
                .Include(c => c.Customer)
                .Include(c => c.ShippingAddress)
                .Include(c => c.Payment)
                .Include(c => c.Delivery)
                .FirstOrDefaultAsync(cancellation);

            if (order == null)
            {
                context.AddFailure(nameof(command.OrderId), "Does not exist");
                return;
            }

            if (!IsValidCombination(command.PaymentMethod, order.Delivery!.DeliveryMethod))
                context.AddFailure(nameof(command.PaymentMethod), "Invalid combination");
        });
    }

    private bool IsValidCombination(PaymentMethod? paymentMethod, DeliveryMethod? deliveryMethod)
    {
        return (paymentMethod, deliveryMethod) switch
        {
            (PaymentMethod.CashOnDelivery, DeliveryMethod.PickupFromBranch) => false,
            (PaymentMethod.CashAtBranch, DeliveryMethod.Standard) => false,
            (PaymentMethod.CashAtBranch, DeliveryMethod.Express) => false,
            (PaymentMethod.CashAtBranch, DeliveryMethod.SameDay) => false,
            (PaymentMethod.CashAtBranch, DeliveryMethod.Scheduled) => false,
            _ => true
        };
    }
}

