using Core.Features.Categories.Commands.Models;

namespace Core.Features.Categories.Commands.Validators
{
    public class EditCategoryValidator : AbstractValidator<EditCategoryCommand>
    {
        private readonly ICategoryService _categoryService;        public EditCategoryValidator(ICategoryService categoryService)
        {
            _categoryService = categoryService;            ApplyValidationRoles();
            ApplyCustomValidationRoles();
        }

        public void ApplyValidationRoles()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required);

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
                .MustAsync(async (model, name, cancellation) => !await _categoryService.IsNameExistExcludeSelf(name, model.Id))
                .WithMessage(SharedResourcesKeys.IsExist);
        }
    }
}
