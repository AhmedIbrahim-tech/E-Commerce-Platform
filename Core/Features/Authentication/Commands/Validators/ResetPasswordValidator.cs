using Core.Features.Authentication.Commands.Models;

namespace Core.Features.Authentication.Commands.Validators;
public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
{
    #region Constructors
    public ResetPasswordValidator()
    {        ApplyValidationRoles();
    }
    #endregion

    #region Handle Functions
    public void ApplyValidationRoles()
    {
        RuleFor(c => c.Email)
            .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
            .NotNull().WithMessage(SharedResourcesKeys.Required)
            .EmailAddress().WithMessage(SharedResourcesKeys.InvalidFormat);

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
