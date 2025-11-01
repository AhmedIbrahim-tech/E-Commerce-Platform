using Core.Features.Carts.Commands.Models;

namespace Core.Features.Carts.Commands.Validators
{
    public class DeleteCartValidator : AbstractValidator<DeleteCartCommand>
    {
        #region Constructors
        public DeleteCartValidator()
        {            ApplyValidationRoles();
        }
        #endregion

        #region Functions Handle
        public void ApplyValidationRoles()
        {
            RuleFor(c => c.CartId)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required);
        }
        #endregion
    }
}