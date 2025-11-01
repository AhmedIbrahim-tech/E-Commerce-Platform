using Core.Features.Authorization.Commands.Models;

namespace Core.Features.Authorization.Commands.Validators
{
    public class AddRoleValidator : AbstractValidator<AddRoleCommand>
    {
        #region Fields
        private readonly IAuthorizationService _authorizationService;
        #endregion

        #region Constructors
        public AddRoleValidator(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;            ApplyValidationRoles();
            ApplyCustomValidationRoles();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationRoles()
        {
            RuleFor(c => c.RoleName)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required)
                .MaximumLength(100).WithMessage(SharedResourcesKeys.MaxLengthIs100);
        }

        public void ApplyCustomValidationRoles()
        {
            RuleFor(c => c.RoleName)
                .MustAsync(async (name, cancellation) => !await _authorizationService.IsRoleExistByName(name))
                .WithMessage(SharedResourcesKeys.IsExist);
        }
        #endregion
    }
}
