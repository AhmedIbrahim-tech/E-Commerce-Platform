using Bogus;
using Domain.Entities.Catalog;
using Domain.Enums;
using Infrastructure.Data;

namespace Infrastructure.Seeder;

public static class ProductSeeder
{
    public static async Task SeedAsync(ApplicationDbContext dbContext, Guid defaultUserId)
    {
        var productsCount = await dbContext.Products.CountAsync();
        if (productsCount > 0)
            return;

        var categories = await dbContext.Categories.ToListAsync(cancellationToken: default);
        var subCategories = await dbContext.SubCategories.ToListAsync(cancellationToken: default);
        var brands = await dbContext.Brands.ToListAsync(cancellationToken: default);
        var units = await dbContext.UnitOfMeasures.ToListAsync(cancellationToken: default);
        var warranties = await dbContext.Warranties.ToListAsync(cancellationToken: default);
        var tags = await dbContext.Tags.ToListAsync(cancellationToken: default);

        if (categories.Count == 0)
            return;

        var productFaker = new Faker<Product>()
            .RuleFor(p => p.Id, f => f.Random.Guid())
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Slug, (f, p) => GenerateSlug(p.Name))
            .RuleFor(p => p.SKU, (f, p) => $"SKU-{f.Random.AlphaNumeric(8).ToUpperInvariant()}")
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.ShortDescription, f => f.Lorem.Sentence(6))
            .RuleFor(p => p.Price, f => f.Random.Decimal(10m, 999m))
            .RuleFor(p => p.StockQuantity, f => f.Random.Int(1, 500))
            .RuleFor(p => p.QuantityAlert, f => f.Random.Int(5, 50))
            .RuleFor(p => p.Barcode, f => f.Commerce.Ean13())
            .RuleFor(p => p.BarcodeSymbology, f => "EAN-13")
            .RuleFor(p => p.Manufacturer, f => f.Company.CompanyName())
            .RuleFor(p => p.IsActive, f => f.Random.Bool(0.9f))
            .RuleFor(p => p.PublishStatus, f => f.PickRandom<ProductPublishStatus>())
            .RuleFor(p => p.Visibility, f => f.PickRandom<ProductVisibility>())
            .RuleFor(p => p.ProductType, f => f.PickRandom<ProductType>())
            .RuleFor(p => p.SellingType, f => f.PickRandom<SellingType>())
            .RuleFor(p => p.TaxType, f => f.PickRandom(new TaxType?[] { null, TaxType.Exclusive, TaxType.Inclusive }))
            .RuleFor(p => p.TaxRate, (f, p) => p.TaxType != null ? f.Random.Decimal(5m, 20m) : null)
            .RuleFor(p => p.DiscountType, f => f.PickRandom(new DiscountType?[] { null, DiscountType.Percentage, DiscountType.Fixed }))
            .RuleFor(p => p.DiscountValue, (f, p) => p.DiscountType != null ? f.Random.Decimal(5m, 30m) : null)
            .RuleFor(p => p.CategoryId, f => f.PickRandom(categories).Id)
            .RuleFor(p => p.SubCategoryId, f => subCategories.Count > 0 && f.Random.Bool(0.6f) ? f.PickRandom(subCategories).Id : null)
            .RuleFor(p => p.BrandId, f => brands.Count > 0 && f.Random.Bool(0.7f) ? f.PickRandom(brands).Id : null)
            .RuleFor(p => p.UnitOfMeasureId, f => units.Count > 0 && f.Random.Bool(0.5f) ? f.PickRandom(units).Id : null)
            .RuleFor(p => p.WarrantyId, f => warranties.Count > 0 && f.Random.Bool(0.4f) ? f.PickRandom(warranties).Id : null)
            .RuleFor(p => p.CreatedTime, f => DateTimeOffset.UtcNow)
            .RuleFor(p => p.CreatedBy, _ => defaultUserId)
            .RuleFor(p => p.PublishDate, (f, p) => p.PublishStatus == ProductPublishStatus.Published ? DateTimeOffset.UtcNow.AddDays(-f.Random.Int(1, 90)) : null)
            .RuleFor(p => p.ManufacturedDate, f => f.Date.PastOffset(2))
            .RuleFor(p => p.ExpiryDate, f => f.Random.Bool(0.3f) ? f.Date.FutureOffset(2) : null)
            .RuleFor(p => p.IsDeleted, _ => false);

        var products = productFaker.Generate(50);

        // Ensure unique Slug and SKU
        var slugSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var skuSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var p in products)
        {
            var baseSlug = p.Slug;
            var c = 0;
            while (!slugSet.Add(p.Slug))
                p.Slug = $"{baseSlug}-{++c}";

            var baseSku = p.SKU;
            c = 0;
            while (!skuSet.Add(p.SKU))
                p.SKU = $"{baseSku}-{++c}";
        }

        await dbContext.Products.AddRangeAsync(products);
        await dbContext.SaveChangesAsync();

        // Add ProductTags for a subset of products
        if (tags.Count > 0)
        {
            var productTags = new List<ProductTag>();
            var productList = await dbContext.Products.Take(30).ToListAsync(cancellationToken: default);
            var tagFaker = new Faker();
            var productTagKeys = new HashSet<(Guid ProductId, Guid TagId)>();
            foreach (var product in productList)
            {
                var tagCount = tagFaker.Random.Int(1, Math.Min(4, tags.Count));
                var selectedTags = tagFaker.PickRandom(tags, tagCount).DistinctBy(t => t.Id).ToList();
                foreach (var tag in selectedTags)
                {
                    if (productTagKeys.Add((product.Id, tag.Id)))
                        productTags.Add(new ProductTag { ProductId = product.Id, TagId = tag.Id });
                }
            }

            await dbContext.ProductTags.AddRangeAsync(productTags);
            await dbContext.SaveChangesAsync();
        }
    }

    private static string GenerateSlug(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return string.Empty;
        return name.ToLowerInvariant()
            .Trim()
            .Replace(" ", "-", StringComparison.Ordinal)
            .Replace("&", "and", StringComparison.Ordinal);
    }
}
