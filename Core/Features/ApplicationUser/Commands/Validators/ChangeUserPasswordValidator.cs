using Core.Features.ApplicationUser.Commands.Models;

namespace Core.Features.ApplicationUser.Commands.Validators
{
    public class ChangeUserPasswordValidator : AbstractValidator<ChangeUserPasswordCommand>
    {

        #region Constructors
        public ChangeUserPasswordValidator()
        {            ApplyValidationRoles();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationRoles()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required);

            RuleFor(c => c.CurrentPassword)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required);

            RuleFor(c => c.NewPassword)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .MinimumLength(6).WithMessage(SharedResourcesKeys.MinLengthIs6)
                .NotNull().WithMessage(SharedResourcesKeys.Required);

            RuleFor(c => c.ConfirmPassword)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required)
                .Equal(c => c.NewPassword).WithMessage(SharedResourcesKeys.PasswordsDoNotMatch);
        }
        #endregion
    }
}
