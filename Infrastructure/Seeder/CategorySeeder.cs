using Domain.Entities.Catalog;
using Infrastructure.Data;

namespace Infrastructure.Seeder;

public static class CategorySeeder
{
    public static async Task SeedAsync(ApplicationDbContext dbContext)
    {
        var categoriesCount = await dbContext.Categories.CountAsync();
        if (categoriesCount > 0)
            return;

        var categories = new List<Category>
        {
            new Category
            {
                Name = "Electronics",
                Description = "Electronic devices and accessories"
            },
            new Category
            {
                Name = "Clothing",
                Description = "Apparel and fashion items"
            },
            new Category
            {
                Name = "Home & Kitchen",
                Description = "Home appliances and kitchen items"
            },
            new Category
            {
                Name = "Sports & Outdoors",
                Description = "Sports equipment and outdoor gear"
            },
            new Category
            {
                Name = "Books",
                Description = "Books and educational materials"
            },
            new Category
            {
                Name = "Beauty & Personal Care",
                Description = "Beauty products and personal care items"
            },
            new Category
            {
                Name = "Toys & Games",
                Description = "Toys, games, and entertainment"
            },
            new Category
            {
                Name = "Automotive",
                Description = "Automotive parts and accessories"
            },
            new Category
            {
                Name = "Health & Wellness",
                Description = "Health products and wellness items"
            },
            new Category
            {
                Name = "Food & Beverages",
                Description = "Food items and beverages"
            }
        };

        await dbContext.Categories.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync();
    }
}
