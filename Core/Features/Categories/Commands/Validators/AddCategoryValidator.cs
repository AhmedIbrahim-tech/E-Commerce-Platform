using Core.Features.Categories.Commands.Models;

namespace Core.Features.Categories.Commands.Validators
{
    public class AddCategoryValidator : AbstractValidator<AddCategoryCommand>
    {
        private readonly ICategoryService _categoryService;        public AddCategoryValidator(ICategoryService categoryService)
        {
            _categoryService = categoryService;            ApplyValidationRoles();
            ApplyCustomValidationRoles();
        }

        public void ApplyValidationRoles()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required)
                .MaximumLength(100).WithMessage(SharedResourcesKeys.MaxLengthIs100);
            RuleFor(c => c.Description)
                .MaximumLength(300).WithMessage(SharedResourcesKeys.MaxLengthIs300);
        }

        public void ApplyCustomValidationRoles()
        {
            RuleFor(c => c.Name)
                .MustAsync(async (name, cancellation) => !await _categoryService.IsNameExist(name))
                .WithMessage(SharedResourcesKeys.IsExist);
        }
    }
}
