using Core.Features.Payments.Commands.Models;

namespace Core.Features.Payments.Commands.Validators
{
    public class SetPaymentMethodValidator : AbstractValidator<SetPaymentMethodCommand>
    {
        #region Fields
        private readonly IOrderService _orderService;
        #endregion

        #region Constructors
        public SetPaymentMethodValidator( IOrderService orderService)
        {            _orderService = orderService;
            ApplyValidationRoles();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationRoles()
        {
            RuleFor(c => c.OrderId)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required);

            RuleFor(c => c.PaymentMethod)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required);

            RuleFor(c => c).CustomAsync(async (command, context, cancellation) =>
            {
                var order = await _orderService.GetOrderByIdAsync(command.OrderId);
                if (order == null)
                {
                    context.AddFailure(nameof(command.OrderId), SharedResourcesKeys.IsNotExist);
                    return;
                }

                if (!IsValidCombination(command.PaymentMethod, order.Delivery!.DeliveryMethod))
                    context.AddFailure(nameof(command.PaymentMethod), SharedResourcesKeys.InvalidCombination);
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
        #endregion
    }
}