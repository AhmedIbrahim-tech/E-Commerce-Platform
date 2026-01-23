namespace Application.Features.Notifications.Commands.MarkNotificationsRead;

public class MarkNotificationsReadValidator : AbstractValidator<MarkNotificationsReadCommand>
{
    public MarkNotificationsReadValidator()
    {
        RuleFor(x => x)
            .Must(x => x.MarkAll || !string.IsNullOrWhiteSpace(x.Id))
            .WithMessage("Provide Id or set MarkAll=true");

        RuleFor(x => x)
            .Must(x => !(x.MarkAll && !string.IsNullOrWhiteSpace(x.Id)))
            .WithMessage("Provide either Id or MarkAll, not both");
    }
}

