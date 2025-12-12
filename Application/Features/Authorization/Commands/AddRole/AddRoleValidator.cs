using Infrastructure.Data.Identity;

namespace Application.Features.Authorization.Commands.AddRole;

public class AddRoleValidator(RoleManager<AppRole> roleManager) : AbstractValidator<AddRoleCommand>
{

    public void ApplyValidationRoles()
    {
        RuleFor(c => c.RoleName)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .MaximumLength(100).WithMessage("Maximum length is 100 characters");
    }

    public void ApplyCustomValidationRoles()
    {
        RuleFor(c => c.RoleName)
            .MustAsync(async (name, cancellation) => !await roleManager.RoleExistsAsync(name))
            .WithMessage("Already exists");
    }
}

