using Core.Features.Customers.Commands.Models;

namespace Core.Features.Customers.Commands.Validators
{
    public class AddCustomerValidator : AbstractValidator<AddCustomerCommand>
    {
        #region Constructors
        public AddCustomerValidator()
        {            ApplyValidationRoles();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationRoles()
        {
            RuleFor(c => c.FirstName)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required)
                .MaximumLength(100).WithMessage(SharedResourcesKeys.MaxLengthIs100);
            RuleFor(c => c.LastName)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required)
                .MaximumLength(50).WithMessage(SharedResourcesKeys.MaxLengthIs100);
            RuleFor(c => c.UserName)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required)
                .MaximumLength(50).WithMessage(SharedResourcesKeys.MaxLengthIs100);
            RuleFor(c => c.Email)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required)
                .EmailAddress().WithMessage(SharedResourcesKeys.InvalidFormat);
            RuleFor(c => c.Password)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required)
                .MinimumLength(6).WithMessage(SharedResourcesKeys.MinLengthIs6);
            RuleFor(c => c.ConfirmPassword)
                .Equal(c => c.Password).WithMessage(SharedResourcesKeys.PasswordsDoNotMatch);
        }
        #endregion
    }
}
