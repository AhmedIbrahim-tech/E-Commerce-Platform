using Core.Features.Products.Queries.Models;

namespace Core.Features.Products.Queries.Validators
{
    public class GetProductByIdValidator : AbstractValidator<GetProductByIdQuery>
    {
        #region Constructors
        public GetProductByIdValidator()
        {            ApplyValidationRoles();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationRoles()
        {
            RuleFor(c => c.ProductId)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required);
        }
        #endregion
    }
}
