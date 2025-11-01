using Core.Features.Orders.Commands.Models;

namespace Core.Features.Orders.Commands.Validators
{
    public class PlaceOrderValidator : AbstractValidator<PlaceOrderCommand>
    {
        #region Constructors
        public PlaceOrderValidator()
        {            ApplyValidationRoles();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationRoles()
        {
            RuleFor(c => c.OrderId)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required);
        }
        #endregion
    }
}
