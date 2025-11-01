using Core.Features.Notifications.Commands.Models;

namespace Core.Features.Notifications.Commands.Validators
{
    public class EditSingleNotificationToAsReadValidator : AbstractValidator<EditSingleNotificationToAsReadCommand>
    {
        #region Constructors
        public EditSingleNotificationToAsReadValidator()
        {            ApplyValidationRoles();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationRoles()
        {
            RuleFor(c => c.notificationId)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required);
        }
        #endregion
    }
}