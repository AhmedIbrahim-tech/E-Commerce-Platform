using Core.Features.Reviews.Queries.Models;

namespace Core.Features.Reviews.Queries.Validators
{
    public class GetReviewPaginatedListValidator : AbstractValidator<GetReviewPaginatedListQuery>
    {
        #region Constructors
        public GetReviewPaginatedListValidator(IProductService productService, ICategoryService categoryService)
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
