using Domain.Entities.Reviews;
using Domain.Enums.Sorting;

namespace Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetProductByIdQuery, ApiResponse<GetProductByIdResponse>>
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

    public async Task<ApiResponse<GetProductByIdResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.Products.GetTableNoTracking()
            .Where(c => c.Id.Equals(request.ProductId))
            .Include(c => c.Category)
            .Include(c => c.SubCategory)
            .Include(c => c.Brand)
            .Include(c => c.UnitOfMeasure)
            .Include(c => c.Warranty)
            .Include(c => c.ProductImages)
            .Include(c => c.ProductVariants)
            .Include(c => c.ProductTags)
            .ThenInclude(pt => pt.Tag)
            .FirstOrDefaultAsync(cancellationToken);

        if (product is null) return NotFound<GetProductByIdResponse>("Product not found");

        var isActive = ComputeIsActive(product.PublishStatus, product.PublishDate);

        var productResponse = new GetProductByIdResponse(
            product.Id,
            product.Name,
            product.Slug,
            product.SKU,
            product.Description,
            product.ShortDescription,
            product.Price,
            product.StockQuantity,
            product.QuantityAlert,
            product.Barcode,
            product.BarcodeSymbology,
            product.PublishStatus,
            product.Visibility,
            product.PublishDate,
            product.ProductType,
            product.SellingType,
            product.TaxType,
            product.TaxRate,
            product.DiscountType,
            product.DiscountValue,
            product.CategoryId,
            product.Category?.Name,
            product.SubCategoryId,
            product.SubCategory?.Name,
            product.BrandId,
            product.Brand?.Name,
            product.UnitOfMeasureId,
            product.UnitOfMeasure?.Name,
            product.WarrantyId,
            product.Warranty?.Name,
            product.ManufacturedDate,
            product.ExpiryDate,
            product.Manufacturer,
            isActive,
            product.CreatedTime,
            product.ModifiedTime,
            product.ProductImages.Select(pi => new ProductImageResponse(
                pi.Id,
                pi.ImageURL,
                pi.IsPrimary,
                pi.DisplayOrder)).ToList(),
            product.ProductVariants.Select(pv => new ProductVariantResponse(
                pv.Id,
                pv.VariantAttribute,
                pv.VariantValue,
                pv.SKU,
                pv.Quantity,
                pv.Price,
                pv.ImageURL,
                pv.IsActive)).ToList(),
            product.ProductTags
                .Where(pt => pt.Tag.IsActive)
                .Select(pt => pt.Tag.Name)
                .OrderBy(n => n)
                .ToList()
        );

        Expression<Func<Review, ReviewResponse>> expression = review => new ReviewResponse(
            review.CustomerId,
            review.Customer != null ? review.Customer.FullName : null,
            review.Rating,
            review.Comment
        );

        IQueryable<Review> queryable = unitOfWork.Reviews.GetTableNoTracking()
            .Where(r => r.ProductId == request.ProductId)
            .Include(r => r.Customer)
            .Include(r => r.Product);

        if (!string.IsNullOrWhiteSpace(request.Search))
            queryable = queryable.Where(c => c.Comment!.Contains(request.Search!));

        queryable = request.SortBy switch
        {
            ReviewSortingEnum.CreatedDateAsc => queryable.OrderBy(c => c.CreatedTime),
            ReviewSortingEnum.CreatedDateDesc => queryable.OrderByDescending(c => c.CreatedTime),
            ReviewSortingEnum.RatingAsc => queryable.OrderBy(c => c.Rating),
            ReviewSortingEnum.RatingDesc => queryable.OrderByDescending(c => c.Rating),
            _ => queryable.OrderByDescending(c => c.CreatedTime)
                          .ThenByDescending(c => c.Rating)
        };

        var reviewPaginatedList = await queryable.Select(expression)
                                                        .ToPaginatedListAsync(request.ReviewPageNumber, request.ReviewPageSize);
        productResponse.Reviews = reviewPaginatedList;

        return Success(productResponse);
    }
}

