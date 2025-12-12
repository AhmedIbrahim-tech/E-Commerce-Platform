namespace Application.Features.Authorization.Commands.DeleteRole;

public class DeleteRoleValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleValidator()
    {
        ApplyValidationRoles();
    }

    public void ApplyValidationRoles()
    {
        RuleFor(c => c.RoleId)
            .NotEmpty().WithMessage("NotEmpty")
            .NotNull().WithMessage("Required");
    }
}

