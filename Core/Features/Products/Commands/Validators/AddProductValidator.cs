using Core.Features.Products.Commands.Models;

namespace Core.Features.Products.Commands.Validators
{
    public class AddProductValidator : AbstractValidator<AddProductCommand>
    {
        #region Fields
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        #endregion

        #region Constructors
        public AddProductValidator(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;            
            ApplyValidationRoles();
            ApplyCustomValidationRoles();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationRoles()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required)
                .MaximumLength(100).WithMessage(SharedResourcesKeys.MaxLengthIs100);

            RuleFor(c => c.Description)
                .MaximumLength(300).WithMessage(SharedResourcesKeys.MaxLengthIs300);

            RuleFor(c => c.Price)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty);

            RuleFor(c => c.StockQuantity)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty);

            RuleFor(c => c.CategoryId)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required);
        }

        public void ApplyCustomValidationRoles()
        {
            RuleFor(c => c.Name)
                .MustAsync(async (name, cancellation) => !await _productService.IsNameExist(name))
                .WithMessage(SharedResourcesKeys.IsExist);

            RuleFor(c => c.CategoryId)
                .MustAsync(async (key, cancellation) => await _categoryService.IsCategoryIdExist(key))
                .WithMessage(SharedResourcesKeys.IsNotExist);
        }
        #endregion
    }
}
