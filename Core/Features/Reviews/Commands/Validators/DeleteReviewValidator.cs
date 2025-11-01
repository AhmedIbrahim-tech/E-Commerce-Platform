using Core.Features.Reviews.Commands.Models;

namespace Core.Features.Reviews.Commands.Validators
{
    internal class DeleteReviewValidator : AbstractValidator<DeleteReviewCommand>
    {
        #region Constructors
        public DeleteReviewValidator()
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
