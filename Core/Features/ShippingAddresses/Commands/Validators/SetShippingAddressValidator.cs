using Core.Features.ShippingAddresses.Commands.Models;

namespace Core.Features.ShippingAddresses.Commands.Validators
{
    public class SetShippingAddressValidator : AbstractValidator<SetShippingAddressCommand>
    {
        #region Constructors
        public SetShippingAddressValidator()
        {            ApplyValidationRoles();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationRoles()
        {
            RuleFor(c => c.OrderId)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required);

            RuleFor(c => c.ShippingAddressId)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required);
        }
        #endregion
    }
}