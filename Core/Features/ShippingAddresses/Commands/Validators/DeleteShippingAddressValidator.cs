using Core.Features.ShippingAddresses.Commands.Models;

namespace Core.Features.ShippingAddresses.Commands.Validators
{
    internal class DeleteShippingAddressValidator : AbstractValidator<DeleteShippingAddressCommand>
    {
        #region Constructors
        public DeleteShippingAddressValidator( IShippingAddressService shippingAddressService)
        {            ApplyValidationRoles();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationRoles()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required);
        }
        #endregion
    }
}
