using Core.Features.Authentication.Commands.Models;

namespace Core.Features.Authentication.Commands.Validators
{
    internal class GoogleLoginValidator : AbstractValidator<GoogleLoginCommand>
    {
        #region Constructors
        public GoogleLoginValidator()
        {            ApplyValidationRoles();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationRoles()
        {
            RuleFor(c => c.IdToken)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required);
        }
        #endregion
    }
}
