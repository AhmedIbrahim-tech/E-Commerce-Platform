using Application.Common.Bases;
using Application.Common.Constants;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Products.Commands.EditProduct;

public class EditProductCommandHandler(
    IUnitOfWork unitOfWork,
    IFileUploadService fileUploadService) : ApiResponseHandler(),
    IRequestHandler<EditProductCommand, ApiResponse<string>>
{
    private static bool ComputeIsActive(ProductPublishStatus status, DateTimeOffset? publishDate)
    {
        var now = DateTimeOffset.UtcNow;
        return status switch
        {
            ProductPublishStatus.Published => true,
            ProductPublishStatus.Scheduled => publishDate.HasValue && publishDate.Value <= now,
            _ => false
        };
    }

    public async Task<ApiResponse<string>> Handle(EditProductCommand request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.Products.GetTableAsTracking()
            .Where(p => p.Id.Equals(request.Id))
            .Include(p => p.Category)
            .Include(p => p.ProductImages)
            .Include(p => p.ProductVariants)
            .Include(p => p.ProductTags)
            .FirstOrDefaultAsync(cancellationToken);

        if (product == null)
            return NotFound<string>("Product not found");

        var existingProduct = await unitOfWork.Products.GetTableNoTracking()
            .AnyAsync(p => (p.SKU == request.SKU || p.Slug == request.Slug) && p.Id != request.Id, cancellationToken);

        if (existingProduct)
            return BadRequest<string>("Product with this SKU or Slug already exists");

        var categoryExists = await unitOfWork.Categories.GetTableNoTracking()
            .AnyAsync(c => c.Id == request.CategoryId, cancellationToken);

        if (!categoryExists)
            return NotFound<string>("Category not found");

        if (request.SubCategoryId.HasValue)
        {
            var subCategoryExists = await unitOfWork.SubCategories.GetTableNoTracking()
                .AnyAsync(sc => sc.Id == request.SubCategoryId.Value, cancellationToken);

            if (!subCategoryExists)
                return NotFound<string>("SubCategory not found");
        }

        if (request.BrandId.HasValue)
        {
            var brandExists = await unitOfWork.Brands.GetTableNoTracking()
                .AnyAsync(b => b.Id == request.BrandId.Value, cancellationToken);

            if (!brandExists)
                return NotFound<string>("Brand not found");
        }

        var uploadedImageUrls = new List<string>();
        var uploadedVariantImageUrls = new List<string>();

        if (request.ProductImages != null && request.ProductImages.Any())
        {
            var imageFiles = request.ProductImages.Select(img => img.ImageFile).ToList();
            var uploadedPaths = await fileUploadService.UploadAndGetUrlsAsync(
                imageFiles,
                FileLocations.Products,
                product.Id,
                childFolder: null,
                overwrite: false,
                cancellationToken: cancellationToken);

            if (uploadedPaths.Count != imageFiles.Count)
            {
                return BadRequest<string>("Failed to upload one or more product images");
            }

            uploadedImageUrls.AddRange(uploadedPaths);
        }

        if (request.ProductVariants != null && request.ProductVariants.Any())
        {
            var variantImageFiles = request.ProductVariants
                .Where(v => v.ImageURL != null)
                .Select(v => v.ImageURL!)
                .ToList();

            if (variantImageFiles.Any())
            {
                var uploadedVariantPaths = await fileUploadService.UploadAndGetUrlsAsync(
                    variantImageFiles,
                    FileLocations.Products,
                    product.Id,
                    childFolder: null,
                    overwrite: false,
                    cancellationToken: cancellationToken);

                int variantIndex = 0;
                foreach (var variantDto in request.ProductVariants)
                {
                    if (variantDto.ImageURL != null)
                    {
                        if (variantIndex < uploadedVariantPaths.Count)
                            uploadedVariantImageUrls.Add(uploadedVariantPaths[variantIndex++]);
                        else
                            uploadedVariantImageUrls.Add(string.Empty);
                    }
                    else
                    {
                        uploadedVariantImageUrls.Add(string.Empty);
                    }
                }
            }
            else
            {
                uploadedVariantImageUrls.AddRange(Enumerable.Repeat(string.Empty, request.ProductVariants.Count));
            }
        }

        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var isActive = ComputeIsActive(request.PublishStatus, request.PublishDate);

            product.Name = request.Name;
            product.Slug = request.Slug;
            product.SKU = request.SKU;
            product.Description = request.Description;
            product.ShortDescription = request.ShortDescription;
            product.Price = request.Price;
            product.StockQuantity = request.StockQuantity;
            product.QuantityAlert = request.QuantityAlert;
            product.Barcode = request.Barcode;
            product.BarcodeSymbology = request.BarcodeSymbology;
            product.PublishStatus = request.PublishStatus;
            product.Visibility = request.Visibility;
            product.PublishDate = request.PublishDate;
            product.ProductType = request.ProductType;
            product.SellingType = request.SellingType;
            product.TaxType = request.TaxType;
            product.TaxRate = request.TaxRate;
            product.DiscountType = request.DiscountType;
            product.DiscountValue = request.DiscountValue;
            product.CategoryId = request.CategoryId;
            product.SubCategoryId = request.SubCategoryId;
            product.BrandId = request.BrandId;
            product.UnitOfMeasureId = request.UnitOfMeasureId;
            product.WarrantyId = request.WarrantyId;
            product.ManufacturedDate = request.ManufacturedDate;
            product.ExpiryDate = request.ExpiryDate;
            product.Manufacturer = request.Manufacturer;
            product.IsActive = isActive;
            product.ModifiedTime = DateTimeOffset.UtcNow;

            if (request.ReplaceTags)
            {
                var existingProductTags = product.ProductTags.ToList();
                if (existingProductTags.Count > 0)
                {
                    unitOfWork.Context.Set<ProductTag>().RemoveRange(existingProductTags);
                }

                var normalizedTags = (request.Tags ?? new List<string>())
                    .Select(t => t?.Trim())
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                if (normalizedTags.Count > 0)
                {
                    var existingTags = await unitOfWork.Tags.GetTableAsTracking()
                        .Where(t => t.IsActive && normalizedTags.Contains(t.Name))
                        .ToListAsync(cancellationToken);

                    var existingSet = existingTags
                        .Select(t => t.Name)
                        .ToHashSet(StringComparer.OrdinalIgnoreCase);

                    var newTags = normalizedTags
                        .Where(t => !existingSet.Contains(t!))
                        .Select(t => new Tag { Name = t!, IsActive = true })
                        .ToList();

                    if (newTags.Count > 0)
                        await unitOfWork.Tags.AddRangeAsync(newTags, cancellationToken);

                    var allTags = existingTags.Concat(newTags).ToList();
                    product.ProductTags = allTags
                        .Select(t => new ProductTag { ProductId = product.Id, TagId = t.Id, Tag = t })
                        .ToList();
                }
            }

            if (request.ProductImages != null && request.ProductImages.Any())
            {
                var existingImages = product.ProductImages.ToList();
                foreach (var existingImage in existingImages)
                {
                    unitOfWork.Context.Set<ProductImage>().Remove(existingImage);
                }

                var productImages = new List<ProductImage>();
                for (int i = 0; i < request.ProductImages.Count; i++)
                {
                    var imageDto = request.ProductImages[i];
                    productImages.Add(new ProductImage
                    {
                        ProductId = product.Id,
                        ImageURL = uploadedImageUrls[i],
                        IsPrimary = imageDto.IsPrimary || i == 0,
                        DisplayOrder = imageDto.DisplayOrder != 0 ? imageDto.DisplayOrder : i,
                        CreatedTime = DateTimeOffset.UtcNow
                    });
                }
                product.ProductImages = productImages;
            }

            if (request.ProductVariants != null && request.ProductVariants.Any())
            {
                var existingVariants = product.ProductVariants.ToList();
                foreach (var existingVariant in existingVariants)
                {
                    unitOfWork.Context.Set<ProductVariant>().Remove(existingVariant);
                }

                var productVariants = new List<ProductVariant>();
                for (int i = 0; i < request.ProductVariants.Count; i++)
                {
                    var variantDto = request.ProductVariants[i];
                    productVariants.Add(new ProductVariant
                    {
                        ProductId = product.Id,
                        VariantAttribute = variantDto.VariantAttribute,
                        VariantValue = variantDto.VariantValue,
                        SKU = variantDto.SKU,
                        Quantity = variantDto.Quantity,
                        Price = variantDto.Price,
                        ImageURL = !string.IsNullOrEmpty(uploadedVariantImageUrls[i]) ? uploadedVariantImageUrls[i] : null,
                        IsActive = true,
                        CreatedTime = DateTimeOffset.UtcNow
                    });
                }
                product.ProductVariants = productVariants;
            }

            await unitOfWork.Products.UpdateAsync(product, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return Edit(product.Id.ToString(), "Product updated successfully");
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return BadRequest<string>($"Failed to update product: {ex.Message}");
        }
    }
}

