using Core.Features.Authentication.Commands.Models;

namespace Core.Features.Authentication.Commands.Validators
{
    public class SendResetPasswordValidator : AbstractValidator<SendResetPasswordCommand>
    {
        #region Constructors
        public SendResetPasswordValidator()
        {            ApplyValidationRoles();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationRoles()
        {
            RuleFor(c => c.Email)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required);
        }
        #endregion
    }
}
