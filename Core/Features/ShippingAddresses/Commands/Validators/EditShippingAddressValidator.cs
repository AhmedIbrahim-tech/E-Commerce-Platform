using Core.Features.ShippingAddresses.Commands.Models;

namespace Core.Features.ShippingAddresses.Commands.Validators
{
    public class EditShippingAddressValidator : AbstractValidator<EditShippingAddressCommand>
    {
        #region Fields
        private readonly IShippingAddressService _shippingAddressService;
        #endregion

        #region Constructors
        public EditShippingAddressValidator( IShippingAddressService shippingAddressService)
        {            _shippingAddressService = shippingAddressService;
            ApplyValidationRoles();
            ApplyCustomValidationRoles();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationRoles()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required);

            RuleFor(c => c.FirstName)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required)
                .MaximumLength(100).WithMessage(SharedResourcesKeys.MaxLengthIs100);

            RuleFor(c => c.LastName)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required)
                .MaximumLength(100).WithMessage(SharedResourcesKeys.MaxLengthIs100);

            RuleFor(c => c.Street)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required)
                .MaximumLength(100).WithMessage(SharedResourcesKeys.MaxLengthIs100);

            RuleFor(c => c.City)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required)
                .MaximumLength(100).WithMessage(SharedResourcesKeys.MaxLengthIs100);

            RuleFor(c => c.State)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required)
                .MaximumLength(100).WithMessage(SharedResourcesKeys.MaxLengthIs100);
        }

        public void ApplyCustomValidationRoles()
        {
            RuleFor(c => c)
                .MustAsync(async (model, shippingAddress, cancellation) => !await _shippingAddressService.IsShippingAddressExistExcludeSelf(shippingAddress.Street!, shippingAddress.City!, shippingAddress.State!, model.Id))
                .WithMessage(SharedResourcesKeys.ShippingAddressIsExist);
        }
        #endregion
    }
}
