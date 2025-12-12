using Infrastructure.Data.Identity;

namespace Application.Features.Authorization.Commands.EditRole;

public class EditRoleValidator(RoleManager<AppRole> roleManager) : AbstractValidator<EditRoleCommand>
{

    public void ApplyValidationRoles()
    {
        RuleFor(c => c.RoleId)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required");

        RuleFor(c => c.RoleName)
            .NotEmpty().WithMessage("Field cannot be empty")
            .NotNull().WithMessage("Field is required")
            .MaximumLength(100).WithMessage("Maximum length is 100 characters");
    }

    public void ApplyCustomValidationRoles()
    {
        RuleFor(c => c.RoleName)
            .MustAsync(async (model, name, cancellation) =>
            {
                var result1 = await roleManager.FindByIdAsync(model.RoleId.ToString());
                if (result1 is null) return false;
                var result2 = await roleManager.RoleExistsAsync(name);
                if (result2 == true && result1.Name == name)
                    return true;
                return false;
            })
            .WithMessage("Already exists");
    }
}

