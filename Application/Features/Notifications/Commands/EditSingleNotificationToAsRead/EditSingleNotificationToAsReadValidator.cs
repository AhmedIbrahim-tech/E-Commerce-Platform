namespace Application.Features.Notifications.Commands.EditSingleNotificationToAsRead;

public class EditSingleNotificationToAsReadValidator : AbstractValidator<EditSingleNotificationToAsReadCommand>
{
    public EditSingleNotificationToAsReadValidator()
    {
        ApplyValidationRules();
    }

    public void ApplyValidationRules()
    {
        RuleFor(c => c.notificationId)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required");
    }
}

