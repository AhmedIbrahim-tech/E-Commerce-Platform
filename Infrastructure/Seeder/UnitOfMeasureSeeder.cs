using Domain.Entities.Catalog;
using Infrastructure.Data;

namespace Infrastructure.Seeder;

public static class UnitOfMeasureSeeder
{
    public static async Task SeedAsync(ApplicationDbContext dbContext, Guid defaultUserId)
    {
        var unitsCount = await dbContext.UnitOfMeasures.CountAsync();
        if (unitsCount > 0)
            return;

        var units = new List<UnitOfMeasure>
        {
            new UnitOfMeasure
            {
                Name = "Piece",
                ShortName = "PCS",
                Description = "Individual item or piece",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            },
            new UnitOfMeasure
            {
                Name = "Kilogram",
                ShortName = "KG",
                Description = "Weight measurement in kilograms",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            },
            new UnitOfMeasure
            {
                Name = "Gram",
                ShortName = "G",
                Description = "Weight measurement in grams",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            },
            new UnitOfMeasure
            {
                Name = "Liter",
                ShortName = "L",
                Description = "Volume measurement in liters",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            },
            new UnitOfMeasure
            {
                Name = "Milliliter",
                ShortName = "ML",
                Description = "Volume measurement in milliliters",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            },
            new UnitOfMeasure
            {
                Name = "Meter",
                ShortName = "M",
                Description = "Length measurement in meters",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            },
            new UnitOfMeasure
            {
                Name = "Centimeter",
                ShortName = "CM",
                Description = "Length measurement in centimeters",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            },
            new UnitOfMeasure
            {
                Name = "Box",
                ShortName = "BOX",
                Description = "Packaged in a box",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            },
            new UnitOfMeasure
            {
                Name = "Pack",
                ShortName = "PK",
                Description = "Packaged items",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            },
            new UnitOfMeasure
            {
                Name = "Set",
                ShortName = "SET",
                Description = "A set of items",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            }
        };

        await dbContext.UnitOfMeasures.AddRangeAsync(units);
        await dbContext.SaveChangesAsync();
    }
}
