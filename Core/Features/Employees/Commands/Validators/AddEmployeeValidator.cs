using Core.Features.Employees.Commands.Models;

namespace Core.Features.Employees.Commands.Validators
{
    public class AddEmployeeValidator : AbstractValidator<AddEmployeeCommand>
    {
        #region Constructors
        public AddEmployeeValidator()
        {            ApplyValidationRoles();
        }
        #endregion

        #region Functions
        public void ApplyValidationRoles()
        {
            RuleFor(e => e.FirstName)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required)
                .MaximumLength(100).WithMessage(SharedResourcesKeys.MaxLengthIs100);

            RuleFor(e => e.LastName)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required)
                .MaximumLength(50).WithMessage(SharedResourcesKeys.MaxLengthIs100);

            RuleFor(e => e.UserName)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required)
                .MaximumLength(50).WithMessage(SharedResourcesKeys.MaxLengthIs100);

            RuleFor(e => e.Email)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required)
                .EmailAddress().WithMessage(SharedResourcesKeys.InvalidFormat);

            RuleFor(e => e.Password)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required)
                .MinimumLength(6).WithMessage(SharedResourcesKeys.MinLengthIs6);

            RuleFor(e => e.ConfirmPassword)
                .Equal(e => e.Password).WithMessage(SharedResourcesKeys.PasswordsDoNotMatch);

            RuleFor(e => e.Position)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required)
                .MaximumLength(100).WithMessage(SharedResourcesKeys.MaxLengthIs100);

            RuleFor(e => e.Salary)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required)
                .GreaterThan(0).WithMessage(SharedResourcesKeys.GreaterThanZero);

            RuleFor(e => e.Address)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required)
                .MaximumLength(200).WithMessage(SharedResourcesKeys.MaxLengthIs200);
        }
        #endregion
    }
}
