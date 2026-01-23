using Domain.Enums;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Products.Commands.EditProduct;

public class EditProductValidator : AbstractValidator<EditProductCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public EditProductValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        ApplyValidationRules();
        ApplyCustomValidationRules();
    }

    public void ApplyValidationRules()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Product ID is required")
            .NotNull().WithMessage("Product ID is required");

        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Product name is required")
            .NotNull().WithMessage("Product name is required")
            .MaximumLength(200).WithMessage("Maximum length is 200 characters");

        RuleFor(c => c.Slug)
            .NotEmpty().WithMessage("Slug is required")
            .NotNull().WithMessage("Slug is required")
            .MaximumLength(250).WithMessage("Maximum length is 250 characters")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Slug must be lowercase alphanumeric with hyphens");

        RuleFor(c => c.SKU)
            .NotEmpty().WithMessage("SKU is required")
            .NotNull().WithMessage("SKU is required")
            .MaximumLength(100).WithMessage("Maximum length is 100 characters");

        RuleFor(c => c.Description)
            .MaximumLength(2000).WithMessage("Maximum length is 2000 characters");

        RuleFor(c => c.ShortDescription)
            .MaximumLength(500).WithMessage("Maximum length is 500 characters");

        RuleFor(c => c.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");

        RuleFor(c => c.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative");

        RuleFor(c => c.QuantityAlert)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity alert cannot be negative");

        RuleFor(c => c.CategoryId)
            .NotEmpty().WithMessage("Category is required")
            .NotNull().WithMessage("Category is required");

        RuleFor(c => c.ProductType)
            .IsInEnum().WithMessage("Invalid product type");

        RuleFor(c => c.SellingType)
            .IsInEnum().WithMessage("Invalid selling type");

        RuleFor(c => c.PublishStatus)
            .IsInEnum().WithMessage("Invalid publish status");

        RuleFor(c => c.Visibility)
            .IsInEnum().WithMessage("Invalid visibility");

        When(c => c.PublishStatus == ProductPublishStatus.Scheduled, () =>
        {
            RuleFor(c => c.PublishDate)
                .NotNull().WithMessage("Publish date is required when status is Scheduled");
        });

        When(c => c.TaxType.HasValue, () =>
        {
            RuleFor(c => c.TaxRate)
                .GreaterThanOrEqualTo(0).WithMessage("Tax rate cannot be negative")
                .LessThanOrEqualTo(100).WithMessage("Tax rate cannot exceed 100%");
        });

        When(c => c.DiscountType.HasValue, () =>
        {
            RuleFor(c => c.DiscountValue)
                .GreaterThanOrEqualTo(0).WithMessage("Discount value cannot be negative");

            When(c => c.DiscountType == DiscountType.Percentage, () =>
            {
                RuleFor(c => c.DiscountValue)
                    .LessThanOrEqualTo(100).WithMessage("Discount percentage cannot exceed 100%");
            });
        });

        When(c => c.ProductVariants != null && c.ProductVariants.Any(), () =>
        {
            RuleForEach(c => c.ProductVariants)
                .ChildRules(variant =>
                {
                    variant.RuleFor(v => v.VariantAttribute)
                        .NotEmpty().WithMessage("Variant attribute is required");

                    variant.RuleFor(v => v.VariantValue)
                        .NotEmpty().WithMessage("Variant value is required");

                    variant.RuleFor(v => v.SKU)
                        .NotEmpty().WithMessage("Variant SKU is required");

                    variant.RuleFor(v => v.Quantity)
                        .GreaterThanOrEqualTo(0).WithMessage("Variant quantity cannot be negative");

                    variant.RuleFor(v => v.Price)
                        .GreaterThan(0).WithMessage("Variant price must be greater than 0");
                });
        });

        When(c => c.Tags != null && c.Tags.Any(), () =>
        {
            RuleForEach(c => c.Tags)
                .NotEmpty().WithMessage("Tag cannot be empty")
                .MaximumLength(50).WithMessage("Maximum tag length is 50 characters");
        });
    }

    public void ApplyCustomValidationRules()
    {
        RuleFor(c => c.Name)
            .MustAsync(async (model, name, cancellation) => !await _unitOfWork.Products.GetTableNoTracking()
                .AnyAsync(p => p.Name.Equals(name) && p.Id != model.Id, cancellation))
            .WithMessage("Product with this name already exists");

        RuleFor(c => c.Slug)
            .MustAsync(async (model, slug, cancellation) => !await _unitOfWork.Products.GetTableNoTracking()
                .AnyAsync(p => p.Slug.Equals(slug) && p.Id != model.Id, cancellation))
            .WithMessage("Product with this slug already exists");

        RuleFor(c => c.SKU)
            .MustAsync(async (model, sku, cancellation) => !await _unitOfWork.Products.GetTableNoTracking()
                .AnyAsync(p => p.SKU.Equals(sku) && p.Id != model.Id, cancellation))
            .WithMessage("Product with this SKU already exists");

        RuleFor(c => c.CategoryId)
            .MustAsync(async (key, cancellation) => await _unitOfWork.Categories.GetTableNoTracking()
                .AnyAsync(c => c.Id.Equals(key), cancellation))
            .WithMessage("Category does not exist");

        When(c => c.SubCategoryId.HasValue, () =>
        {
            RuleFor(c => c.SubCategoryId)
                .MustAsync(async (key, cancellation) => await _unitOfWork.SubCategories.GetTableNoTracking()
                    .AnyAsync(sc => sc.Id.Equals(key!.Value), cancellation))
                .WithMessage("SubCategory does not exist");
        });

        When(c => c.BrandId.HasValue, () =>
        {
            RuleFor(c => c.BrandId)
                .MustAsync(async (key, cancellation) => await _unitOfWork.Brands.GetTableNoTracking()
                    .AnyAsync(b => b.Id.Equals(key!.Value), cancellation))
                .WithMessage("Brand does not exist");
        });

        When(c => c.UnitOfMeasureId.HasValue, () =>
        {
            RuleFor(c => c.UnitOfMeasureId)
                .MustAsync(async (key, cancellation) => await _unitOfWork.UnitOfMeasures.GetTableNoTracking()
                    .AnyAsync(u => u.Id.Equals(key!.Value), cancellation))
                .WithMessage("Unit of measure does not exist");
        });
    }
}

