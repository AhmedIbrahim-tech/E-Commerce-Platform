using Core.Features.Customers.Commands.Models;

namespace Core.Features.Customers.Commands.Validators
{
    public class DeleteCustomerValidator : AbstractValidator<DeleteCustomerCommand>
    {
        #region Constructors
        public DeleteCustomerValidator()
        {            ApplyValidationRoles();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationRoles()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required);
        }
        #endregion
    }
}