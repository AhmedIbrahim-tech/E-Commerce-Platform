using Core.Features.Deliveries.Commands.Models;

namespace Core.Features.Deliveries.Commands.Validators
{
    public class SetDeliveryMethodValidator : AbstractValidator<SetDeliveryMethodCommand>
    {
        #region Constructors
        public SetDeliveryMethodValidator()
        {            ApplyValidationRoles();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationRoles()
        {
            RuleFor(c => c.OrderId)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required);

            RuleFor(c => c.DeliveryMethod)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required);
        }
        #endregion
    }
}