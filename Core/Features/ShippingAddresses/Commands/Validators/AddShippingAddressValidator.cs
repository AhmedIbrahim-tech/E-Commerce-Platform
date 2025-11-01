using Core.Features.ShippingAddresses.Commands.Models;

namespace Core.Features.ShippingAddresses.Commands.Validators
{
    public class AddShippingAddressValidator : AbstractValidator<AddShippingAddressCommand>
    {
        #region Fields
        private readonly IShippingAddressService _shippingAddressService;
        #endregion

        #region Constructors
        public AddShippingAddressValidator(IShippingAddressService shippingAddressService)
        {
            _shippingAddressService = shippingAddressService;
            ApplyValidationRoles();
            ApplyCustomValidationRoles();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationRoles()
        {
            RuleFor(c => c.FirstName)
                .NotEmpty().WithMessage("NotEmpty")
                .NotNull().WithMessage("Required")
                .MaximumLength(100).WithMessage("MaxLengthIs100");

            RuleFor(c => c.LastName)
                .NotEmpty().WithMessage("NotEmpty")
                .NotNull().WithMessage("Required")
                .MaximumLength(100).WithMessage("MaxLengthIs100");

            RuleFor(c => c.Street)
                .NotEmpty().WithMessage("NotEmpty")
                .NotNull().WithMessage("Required")
                .MaximumLength(100).WithMessage("MaxLengthIs100");

            RuleFor(c => c.City)
                .NotEmpty().WithMessage("NotEmpty")
                .NotNull().WithMessage("Required")
                .MaximumLength(100).WithMessage("MaxLengthIs100");

            RuleFor(c => c.State)
                .NotEmpty().WithMessage("NotEmpty")
                .NotNull().WithMessage("Required")
                .MaximumLength(100).WithMessage("MaxLengthIs100");

        }

        public void ApplyCustomValidationRoles()
        {
            RuleFor(c => c)
                .MustAsync(async (shippingAddress, cancellation) => !await _shippingAddressService.IsShippingAddressExist(shippingAddress.Street, shippingAddress.City, shippingAddress.State))
                .WithMessage("ShippingAddressIsExist");
        }
        #endregion
    }
}
