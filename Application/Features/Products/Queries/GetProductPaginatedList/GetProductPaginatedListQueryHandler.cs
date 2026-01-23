namespace Application.Features.Products.Queries.GetProductPaginatedList;

public class GetProductPaginatedListQueryHandler(IUnitOfWork unitOfWork) : ApiResponseHandler(),
    IRequestHandler<GetProductPaginatedListQuery, PaginatedResult<GetProductPaginatedListResponse>>
{
    public async Task<PaginatedResult<GetProductPaginatedListResponse>> Handle(GetProductPaginatedListQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Product> queryable = unitOfWork.Products.GetTableNoTracking()
            .Include(p => p.Category)
            .Include(p => p.SubCategory)
            .Include(p => p.Brand)
            .Include(p => p.Warranty)
            .Include(p => p.ProductImages);

        var now = DateTimeOffset.UtcNow;

        var predicate = PredicateBuilder.True<Product>();

        if (request.CategoryId.HasValue)
            predicate = predicate.And(p => p.CategoryId == request.CategoryId.Value);

        if (request.BrandIds is { Count: > 0 })
            predicate = predicate.And(p => p.BrandId.HasValue && request.BrandIds.Contains(p.BrandId.Value));

        if (request.MinPrice.HasValue)
            predicate = predicate.And(p => p.Price >= request.MinPrice.Value);

        if (request.MaxPrice.HasValue)
            predicate = predicate.And(p => p.Price <= request.MaxPrice.Value);

        if (request.MinDiscountPercentage.HasValue)
        {
            predicate = predicate.And(p => p.DiscountType == DiscountType.Percentage
                && p.DiscountValue.HasValue
                && p.DiscountValue.Value >= request.MinDiscountPercentage.Value);
        }

        if (request.MinRating.HasValue)
        {
            predicate = predicate.And(p => p.Reviews.Any()
                && p.Reviews.Max(r => (int)r.Rating) >= request.MinRating.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search!.Trim();
            predicate = predicate.And(p =>
                p.Name.Contains(search)
                || (p.Description != null && p.Description.Contains(search))
                || (p.ShortDescription != null && p.ShortDescription.Contains(search))
                || p.SKU.Contains(search)
                || p.Slug.Contains(search));
        }

        var baseQueryable = queryable.Where(predicate);

        var allCount = await baseQueryable.CountAsync(cancellationToken);

        var publishedCount = await baseQueryable
            .Where(p => p.PublishStatus == ProductPublishStatus.Published
                || (p.PublishStatus == ProductPublishStatus.Scheduled && p.PublishDate.HasValue && p.PublishDate.Value <= now))
            .CountAsync(cancellationToken);
        var draftCount = allCount - publishedCount;

        if (request.IsActive.HasValue)
        {
            if (request.IsActive.Value)
            {
                predicate = predicate.And(p => p.PublishStatus == ProductPublishStatus.Published
                    || (p.PublishStatus == ProductPublishStatus.Scheduled && p.PublishDate.HasValue && p.PublishDate.Value <= now));
            }
            else
            {
                predicate = predicate.And(p => p.PublishStatus != ProductPublishStatus.Published
                    && !(p.PublishStatus == ProductPublishStatus.Scheduled && p.PublishDate.HasValue && p.PublishDate.Value <= now));
            }
        }

        queryable = queryable.Where(predicate);

        queryable = request.SortBy switch
        {
            ProductSortingEnum.NameAsc => queryable.OrderBy(c => c.Name),
            ProductSortingEnum.NameDesc => queryable.OrderByDescending(c => c.Name),
            ProductSortingEnum.PriceAsc => queryable.OrderBy(c => c.Price),
            ProductSortingEnum.PriceDesc => queryable.OrderByDescending(c => c.Price),
            ProductSortingEnum.StockQuantityAsc => queryable.OrderBy(c => c.StockQuantity),
            ProductSortingEnum.StockQuantityDesc => queryable.OrderByDescending(c => c.StockQuantity),
            ProductSortingEnum.CreatedDateAsc => queryable.OrderBy(c => c.CreatedTime),
            ProductSortingEnum.CreatedDateDesc => queryable.OrderByDescending(c => c.CreatedTime),
            ProductSortingEnum.RatingAsc => queryable.OrderBy(c => c.Reviews.Any() ? c.Reviews.Max(r => r.Rating) : 0),
            ProductSortingEnum.RatingDesc => queryable.OrderByDescending(c => c.Reviews.Any() ? c.Reviews.Max(r => r.Rating) : 0),
            _ => queryable.OrderBy(c => c.Name)
        };

        var totalCount = await queryable.CountAsync(cancellationToken);
        var products = await queryable
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var responseData = products.Select(c => new GetProductPaginatedListResponse
        {
            Id = c.Id,
            Name = c.Name,
            Slug = c.Slug,
            SKU = c.SKU,
            Description = c.Description,
            ShortDescription = c.ShortDescription,
            Price = c.Price,
            StockQuantity = c.StockQuantity,
            QuantityAlert = c.QuantityAlert,
            Barcode = c.Barcode,
            ProductType = c.ProductType,
            SellingType = c.SellingType,
            PublishStatus = c.PublishStatus,
            Visibility = c.Visibility,
            PublishDate = c.PublishDate,
            IsActive = ComputeIsActive(c.PublishStatus, c.PublishDate),
            CreatedTime = c.CreatedTime,
            CategoryName = c.Category?.Name,
            SubCategoryName = c.SubCategory?.Name,
            BrandName = c.Brand?.Name,
            WarrantyName = c.Warranty?.Name,
            ProductImages = c.ProductImages.Select(pi => new ProductImageResponse(
                pi.Id,
                pi.ImageURL,
                pi.IsPrimary,
                pi.DisplayOrder)).ToList(),
        }).ToList();

        var paginatedList = PaginatedResult<GetProductPaginatedListResponse>.Success(
            responseData,
            totalCount,
            request.PageNumber,
            request.PageSize
        );

        paginatedList.Meta = new
        {
            responseData.Count,
            AllCount = allCount,
            PublishedCount = publishedCount,
            DraftCount = draftCount
        };

        return paginatedList;
    }

    #region Helper Methods
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
    #endregion
}

