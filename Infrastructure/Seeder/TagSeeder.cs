using Domain.Entities.Catalog;
using Infrastructure.Data;

namespace Infrastructure.Seeder;

public static class TagSeeder
{
    public static async Task SeedAsync(ApplicationDbContext dbContext, Guid defaultUserId)
    {
        var tagsCount = await dbContext.Tags.CountAsync();
        if (tagsCount > 0)
            return;

        var tags = new List<Tag>
        {
            new()
            {
                Name = "Cotton",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            },
            new()
            {
                Name = "New",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            },
            new()
            {
                Name = "Sale",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            },
            new()
            {
                Name = "Featured",
                IsActive = true,
                CreatedTime = DateTimeOffset.UtcNow,
                CreatedBy = defaultUserId
            }
        };

        await dbContext.Tags.AddRangeAsync(tags);
        await dbContext.SaveChangesAsync();
    }
}

