using Domain.Entities.Catalog;
using Infrastructure.Data;

namespace Infrastructure.Seeder;

public static class WarrantySeeder
{
    public static async Task SeedAsync(ApplicationDbContext dbContext, Guid defaultUserId)
    {
        var warrantiesCount = await dbContext.Warranties.CountAsync();
        if (warrantiesCount > 0)
            return;

        var warranties = new List<Warranty>
        {
            new Warranty
            {
                Name = "Replacement Warranty",
                Description = "Covers replacement of faulty items",
                Duration = 2,
                DurationPeriod = "Year",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            },
            new Warranty
            {
                Name = "On-Site Warranty",
                Description = "Product repairs done at the customer's location",
                Duration = 1,
                DurationPeriod = "Year",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            },
            new Warranty
            {
                Name = "Accidental Protection Plan",
                Description = "Coverage for accidental damage",
                Duration = 6,
                DurationPeriod = "Months",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            },
            new Warranty
            {
                Name = "Labor-Only Warranty",
                Description = "Covers only labor costs, not parts",
                Duration = 6,
                DurationPeriod = "Months",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            },
            new Warranty
            {
                Name = "No-Cost Repairs",
                Description = "No charge for repairs during warranty period",
                Duration = 3,
                DurationPeriod = "Months",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            },
            new Warranty
            {
                Name = "Accidental Damage",
                Description = "Coverage for unexpected damage",
                Duration = 6,
                DurationPeriod = "Months",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            },
            new Warranty
            {
                Name = "Wear & Tear Warranty",
                Description = "Covers specific product aging issues",
                Duration = 1,
                DurationPeriod = "Year",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            },
            new Warranty
            {
                Name = "Money-Back Guarantee",
                Description = "Refund within a specified period",
                Duration = 3,
                DurationPeriod = "Months",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            },
            new Warranty
            {
                Name = "Water Damage Warranty",
                Description = "Coverage for water-related issues",
                Duration = 6,
                DurationPeriod = "Months",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            },
            new Warranty
            {
                Name = "Power Surge Protection",
                Description = "Covers damage from power surges",
                Duration = 6,
                DurationPeriod = "Months",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            }
        };

        await dbContext.Warranties.AddRangeAsync(warranties);
        await dbContext.SaveChangesAsync();
    }
}
