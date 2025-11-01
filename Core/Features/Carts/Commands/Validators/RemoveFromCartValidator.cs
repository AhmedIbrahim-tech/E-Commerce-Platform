using Core.Features.Carts.Commands.Models;

namespace Core.Features.Carts.Commands.Validators
{
    public class RemoveFromCartValidator : AbstractValidator<RemoveFromCartCommand>
    {
        #region Constructors
        public RemoveFromCartValidator()
        {            ApplyValidationRoles();
        }
        #endregion

        #region Functions Handles
        public void ApplyValidationRoles()
        {
            RuleFor(c => c.ProductId)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required);
        }
        #endregion
    }
}

