using Core.Features.Authorization.Commands.Models;

namespace Core.Features.Authorization.Commands.Validators
{
    public class EditRoleValidator : AbstractValidator<EditRoleCommand>
    {
        #region Fields
        private readonly IAuthorizationService _authorizationService;
        #endregion

        #region Constructors
        public EditRoleValidator(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;            ApplyValidationRoles();
            ApplyCustomValidationRoles();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationRoles()
        {
            RuleFor(c => c.RoleId)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required);

            RuleFor(c => c.RoleName)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required)
                .MaximumLength(100).WithMessage(SharedResourcesKeys.MaxLengthIs100);
        }

        public void ApplyCustomValidationRoles()
        {
            RuleFor(c => c.RoleName)
                .MustAsync(async (model, name, cancellation) => !await _authorizationService.IsRoleExistExcludeSelf(name, model.RoleId))
                .WithMessage(SharedResourcesKeys.IsExist);
        }
        #endregion
    }
}
