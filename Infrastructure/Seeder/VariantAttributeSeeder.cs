using Domain.Entities.Catalog;
using Infrastructure.Data;

namespace Infrastructure.Seeder;

public static class VariantAttributeSeeder
{
    public static async Task SeedAsync(ApplicationDbContext dbContext, Guid defaultUserId)
    {
        var variantAttributesCount = await dbContext.VariantAttributes.CountAsync();
        if (variantAttributesCount > 0)
            return;

        var colorAttribute = new VariantAttribute
        {
            Name = "Color",
            Description = "Product color variants",
            IsActive = true,
            CreatedTime = DateTimeOffset.UtcNow,
            CreatedBy = defaultUserId,
            Values = new List<VariantAttributeValue>
            {
                new VariantAttributeValue
                {
                    Value = "Red",
                    DisplayName = "Red",
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                },
                new VariantAttributeValue
                {
                    Value = "Blue",
                    DisplayName = "Blue",
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                },
                new VariantAttributeValue
                {
                    Value = "Green",
                    DisplayName = "Green",
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                },
                new VariantAttributeValue
                {
                    Value = "Black",
                    DisplayName = "Black",
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                },
                new VariantAttributeValue
                {
                    Value = "White",
                    DisplayName = "White",
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                },
                new VariantAttributeValue
                {
                    Value = "Yellow",
                    DisplayName = "Yellow",
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                },
                new VariantAttributeValue
                {
                    Value = "Purple",
                    DisplayName = "Purple",
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                },
                new VariantAttributeValue
                {
                    Value = "Orange",
                    DisplayName = "Orange",
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                }
            }
        };

        foreach (var value in colorAttribute.Values)
        {
            value.VariantAttributeId = colorAttribute.Id;
        }

        var sizeAttribute = new VariantAttribute
        {
            Name = "Size",
            Description = "Product size variants",
            IsActive = true,
            CreatedTime = DateTimeOffset.UtcNow,
            CreatedBy = defaultUserId,
            Values = new List<VariantAttributeValue>
            {
                new VariantAttributeValue
                {
                    Value = "XS",
                    DisplayName = "Extra Small",
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                },
                new VariantAttributeValue
                {
                    Value = "S",
                    DisplayName = "Small",
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                },
                new VariantAttributeValue
                {
                    Value = "M",
                    DisplayName = "Medium",
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                },
                new VariantAttributeValue
                {
                    Value = "L",
                    DisplayName = "Large",
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                },
                new VariantAttributeValue
                {
                    Value = "XL",
                    DisplayName = "Extra Large",
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                },
                new VariantAttributeValue
                {
                    Value = "XXL",
                    DisplayName = "Double Extra Large",
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                }
            }
        };

        foreach (var value in sizeAttribute.Values)
        {
            value.VariantAttributeId = sizeAttribute.Id;
        }

        var materialAttribute = new VariantAttribute
        {
            Name = "Material",
            Description = "Product material variants",
            IsActive = true,
            CreatedTime = DateTimeOffset.UtcNow,
            CreatedBy = defaultUserId,
            Values = new List<VariantAttributeValue>
            {
                new VariantAttributeValue
                {
                    Value = "Cotton",
                    DisplayName = "Cotton",
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                },
                new VariantAttributeValue
                {
                    Value = "Polyester",
                    DisplayName = "Polyester",
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                },
                new VariantAttributeValue
                {
                    Value = "Leather",
                    DisplayName = "Leather",
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                },
                new VariantAttributeValue
                {
                    Value = "Plastic",
                    DisplayName = "Plastic",
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                },
                new VariantAttributeValue
                {
                    Value = "Metal",
                    DisplayName = "Metal",
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                },
                new VariantAttributeValue
                {
                    Value = "Wood",
                    DisplayName = "Wood",
                    IsActive = true,
                    CreatedTime = DateTimeOffset.UtcNow,
                    CreatedBy = defaultUserId
                }
            }
        };

        foreach (var value in materialAttribute.Values)
        {
            value.VariantAttributeId = materialAttribute.Id;
        }

        await dbContext.VariantAttributes.AddRangeAsync(new[] { colorAttribute, sizeAttribute, materialAttribute });
        await dbContext.SaveChangesAsync();
    }
}
