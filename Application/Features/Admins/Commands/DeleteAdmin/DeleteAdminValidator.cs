namespace Application.Features.Admins.Commands.DeleteAdmin;

public class DeleteAdminValidator : AbstractValidator<DeleteAdminCommand>
{
    public DeleteAdminValidator()
    {
        ApplyValidationRules();
    }

    public void ApplyValidationRules()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required");
    }
}
