using Core.Features.Authentication.Commands.Models;

namespace Core.Features.Authentication.Commands.Validators
{
    public class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
    {

        #region Constructors
        public RefreshTokenValidator()
        {            ApplyValidationRoles();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationRoles()
        {
            RuleFor(c => c.AccessToken)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required);

            RuleFor(c => c.RefreshToken)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required);
        }
        #endregion
    }
}
