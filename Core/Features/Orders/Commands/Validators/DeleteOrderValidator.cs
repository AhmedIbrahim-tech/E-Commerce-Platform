using Core.Features.Orders.Commands.Models;

namespace Core.Features.Orders.Commands.Validators
{
    public class DeleteOrderValidator : AbstractValidator<DeleteOrderCommand>
    {
        #region Constructors
        public DeleteOrderValidator()
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
