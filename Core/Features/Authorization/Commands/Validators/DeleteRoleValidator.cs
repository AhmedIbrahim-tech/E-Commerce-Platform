using Core.Features.Authorization.Commands.Models;

namespace Core.Features.Authorization.Commands.Validators
{
    public class DeleteRoleValidator : AbstractValidator<DeleteRoleCommand>
    {
        #region Fields
        private readonly IAuthorizationService _authorizationService;
        #endregion

        #region Constructors
        public DeleteRoleValidator(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;            ApplyValidationRoles();
            //ApplyCustomValidationRoles();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationRoles()
        {
            RuleFor(c => c.RoleId)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required);
        }

        //public void ApplyCustomValidationRoles()
        //{
        //    RuleFor(c => c.RoleId)
        //        .MustAsync(async (id, cancellation) => await _authorizationService.IsRoleExistById(id))
        //        .WithMessage(SharedResourcesKeys.IsNotExist);
        //}
        #endregion
    }
}