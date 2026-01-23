using Domain.Entities.Catalog;
using Infrastructure.Data;

namespace Infrastructure.Seeder;

public static class BrandSeeder
{
    public static async Task SeedAsync(ApplicationDbContext dbContext, Guid defaultUserId)
    {
        var brandsCount = await dbContext.Brands.CountAsync();
        if (brandsCount > 0)
            return;

        var brands = new List<Brand>
        {
            new Brand
            {
                Name = "Apple",
                Description = "Technology and consumer electronics",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            },
            new Brand
            {
                Name = "Samsung",
                Description = "Electronics and technology",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            },
            new Brand
            {
                Name = "Nike",
                Description = "Athletic footwear and apparel",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            },
            new Brand
            {
                Name = "Adidas",
                Description = "Sports apparel and footwear",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            },
            new Brand
            {
                Name = "Sony",
                Description = "Electronics and entertainment",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            },
            new Brand
            {
                Name = "LG",
                Description = "Home appliances and electronics",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            },
            new Brand
            {
                Name = "Canon",
                Description = "Cameras and imaging equipment",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            },
            new Brand
            {
                Name = "Dell",
                Description = "Computers and technology",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            },
            new Brand
            {
                Name = "HP",
                Description = "Computers and printers",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            },
            new Brand
            {
                Name = "Generic",
                Description = "Generic brand products",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            }
        };

        await dbContext.Brands.AddRangeAsync(brands);
        await dbContext.SaveChangesAsync();
    }
}
