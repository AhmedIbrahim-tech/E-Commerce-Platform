using Core.Features.Carts.Commands.Models;

namespace Core.Features.Carts.Commands.Validators
{
    public class UpdateItemQuantityValidator : AbstractValidator<UpdateItemQuantityCommand>
    {
        #region Constructors
        public UpdateItemQuantityValidator()
        {            ApplyValidationRoles();
        }
        #endregion

        #region Functions
        public void ApplyValidationRoles()
        {
            RuleFor(c => c.ProductId)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required);

            RuleFor(c => c.Quantity)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required)
                .GreaterThan(0).WithMessage(SharedResourcesKeys.GreaterThanZero);
        }
        #endregion
    }
}
