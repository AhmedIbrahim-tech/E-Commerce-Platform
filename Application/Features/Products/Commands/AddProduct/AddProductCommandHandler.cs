using Application.Common.Bases;
using Application.Common.Constants;
using Application.Common.Errors;
using Infrastructure.RepositoriesHandlers.UnitOfWork;

namespace Application.Features.Products.Commands.AddProduct;

public class AddProductCommandHandler(
    IUnitOfWork unitOfWork,
    IFileUploadService fileUploadService) : ApiResponseHandler(),
    IRequestHandler<AddProductCommand, ApiResponse<string>>
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

    public async Task<ApiResponse<string>> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        var existingProduct = await unitOfWork.Products.GetTableNoTracking()
            .AnyAsync(p => p.SKU == request.SKU || p.Slug == request.Slug, cancellationToken);

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

        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var isActive = ComputeIsActive(request.PublishStatus, request.PublishDate);

            var product = new Product
            {
                Name = request.Name,
                Slug = request.Slug,
                SKU = request.SKU,
                Description = request.Description,
                ShortDescription = request.ShortDescription,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                QuantityAlert = request.QuantityAlert,
                Barcode = request.Barcode,
                BarcodeSymbology = request.BarcodeSymbology,
                PublishStatus = request.PublishStatus,
                Visibility = request.Visibility,
                PublishDate = request.PublishDate,
                ProductType = request.ProductType,
                SellingType = request.SellingType,
                TaxType = request.TaxType,
                TaxRate = request.TaxRate,
                DiscountType = request.DiscountType,
                DiscountValue = request.DiscountValue,
                CategoryId = request.CategoryId,
                SubCategoryId = request.SubCategoryId,
                BrandId = request.BrandId,
                UnitOfMeasureId = request.UnitOfMeasureId,
                WarrantyId = request.WarrantyId,
                ManufacturedDate = request.ManufacturedDate,
                ExpiryDate = request.ExpiryDate,
                Manufacturer = request.Manufacturer,
                IsActive = isActive
            };

            await unitOfWork.Products.AddAsync(product, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

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
                    await unitOfWork.RollbackTransactionAsync(cancellationToken);
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

            if (request.Tags is { Count: > 0 })
            {
                var normalizedTags = request.Tags
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

            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return Created(product.Id.ToString(), "Product created successfully");
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return BadRequest<string>($"Failed to create product: {ex.Message}");
        }
    }
}

