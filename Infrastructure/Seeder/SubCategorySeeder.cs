using Domain.Entities.Catalog;
using Infrastructure.Data;

namespace Infrastructure.Seeder;

public static class SubCategorySeeder
{
    public static async Task SeedAsync(ApplicationDbContext dbContext, Guid defaultUserId)
    {
        var subCategoriesCount = await dbContext.SubCategories.CountAsync();
        if (subCategoriesCount > 0)
            return;

        var categories = await dbContext.Categories.ToListAsync();
        if (categories.Count == 0)
            return;

        var electronicsCategory = categories.FirstOrDefault(c => c.Name == "Electronics");
        var clothingCategory = categories.FirstOrDefault(c => c.Name == "Clothing");
        var homeCategory = categories.FirstOrDefault(c => c.Name == "Home & Kitchen");
        var sportsCategory = categories.FirstOrDefault(c => c.Name == "Sports & Outdoors");
        var booksCategory = categories.FirstOrDefault(c => c.Name == "Books");
        var beautyCategory = categories.FirstOrDefault(c => c.Name == "Beauty & Personal Care");

        var subCategories = new List<SubCategory>();

        if (electronicsCategory != null)
        {
            subCategories.AddRange(new[]
            {
                new SubCategory
                {
                    Name = "Smartphones",
                    Description = "Mobile phones and smartphones",
                    Code = "ELEC-SMART",
                    CategoryId = electronicsCategory.Id,
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                },
                new SubCategory
                {
                    Name = "Laptops",
                    Description = "Laptop computers",
                    Code = "ELEC-LAP",
                    CategoryId = electronicsCategory.Id,
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                },
                new SubCategory
                {
                    Name = "Tablets",
                    Description = "Tablet devices",
                    Code = "ELEC-TAB",
                    CategoryId = electronicsCategory.Id,
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                },
                new SubCategory
                {
                    Name = "Headphones",
                    Description = "Audio headphones and earbuds",
                    Code = "ELEC-HEAD",
                    CategoryId = electronicsCategory.Id,
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                }
            });
        }

        if (clothingCategory != null)
        {
            subCategories.AddRange(new[]
            {
                new SubCategory
                {
                    Name = "Men's Clothing",
                    Description = "Men's apparel",
                    Code = "CLO-MEN",
                    CategoryId = clothingCategory.Id,
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                },
                new SubCategory
                {
                    Name = "Women's Clothing",
                    Description = "Women's apparel",
                    Code = "CLO-WOM",
                    CategoryId = clothingCategory.Id,
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                },
                new SubCategory
                {
                    Name = "Kids' Clothing",
                    Description = "Children's apparel",
                    Code = "CLO-KID",
                    CategoryId = clothingCategory.Id,
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                },
                new SubCategory
                {
                    Name = "Shoes",
                    Description = "Footwear",
                    Code = "CLO-SHO",
                    CategoryId = clothingCategory.Id,
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                }
            });
        }

        if (homeCategory != null)
        {
            subCategories.AddRange(new[]
            {
                new SubCategory
                {
                    Name = "Kitchen Appliances",
                    Description = "Kitchen and cooking appliances",
                    Code = "HOME-KIT",
                    CategoryId = homeCategory.Id,
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                },
                new SubCategory
                {
                    Name = "Furniture",
                    Description = "Home furniture",
                    Code = "HOME-FUR",
                    CategoryId = homeCategory.Id,
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                },
                new SubCategory
                {
                    Name = "Home Decor",
                    Description = "Decorative items for home",
                    Code = "HOME-DEC",
                    CategoryId = homeCategory.Id,
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                }
            });
        }

        if (sportsCategory != null)
        {
            subCategories.AddRange(new[]
            {
                new SubCategory
                {
                    Name = "Fitness Equipment",
                    Description = "Exercise and fitness equipment",
                    Code = "SPO-FIT",
                    CategoryId = sportsCategory.Id,
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                },
                new SubCategory
                {
                    Name = "Outdoor Gear",
                    Description = "Camping and outdoor equipment",
                    Code = "SPO-OUT",
                    CategoryId = sportsCategory.Id,
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                }
            });
        }

        if (booksCategory != null)
        {
            subCategories.AddRange(new[]
            {
                new SubCategory
                {
                    Name = "Fiction",
                    Description = "Fiction books",
                    Code = "BOOK-FIC",
                    CategoryId = booksCategory.Id,
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                },
                new SubCategory
                {
                    Name = "Non-Fiction",
                    Description = "Non-fiction books",
                    Code = "BOOK-NON",
                    CategoryId = booksCategory.Id,
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                },
                new SubCategory
                {
                    Name = "Educational",
                    Description = "Educational and textbooks",
                    Code = "BOOK-EDU",
                    CategoryId = booksCategory.Id,
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                }
            });
        }

        if (beautyCategory != null)
        {
            subCategories.AddRange(new[]
            {
                new SubCategory
                {
                    Name = "Skincare",
                    Description = "Skincare products",
                    Code = "BEA-SKI",
                    CategoryId = beautyCategory.Id,
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                },
                new SubCategory
                {
                    Name = "Makeup",
                    Description = "Cosmetics and makeup",
                    Code = "BEA-MAK",
                    CategoryId = beautyCategory.Id,
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                },
                new SubCategory
                {
                    Name = "Fragrances",
                    Description = "Perfumes and fragrances",
                    Code = "BEA-FRA",
                    CategoryId = beautyCategory.Id,
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                }
            });
        }

        if (subCategories.Count > 0)
        {
            await dbContext.SubCategories.AddRangeAsync(subCategories);
            await dbContext.SaveChangesAsync();
        }
    }
}
