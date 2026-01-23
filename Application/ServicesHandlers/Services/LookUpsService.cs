using Application.Common.DTOs;
using Domain.Enums;

namespace Application.ServicesHandlers.Services;

public class LookUpsService(IUnitOfWork unitOfWork) : ILookUpsService
{
    public async Task<List<BaseLookupDto>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await unitOfWork.Categories.GetTableNoTracking()
            .OrderBy(c => c.Name)
            .Select(c => new BaseLookupDto { Id = c.Id, Name = c.Name })
            .ToListAsync(cancellationToken);
    }

    public async Task<List<BaseLookupDto>> GetSubCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await unitOfWork.SubCategories.GetTableNoTracking()
            .Where(sc => !sc.IsDeleted && sc.IsActive)
            .OrderBy(sc => sc.Name)
            .Select(sc => new BaseLookupDto
            {
                Id = sc.Id,
                Name = sc.Name
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<List<BaseLookupDto>> GetSubCategoriesByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        return await unitOfWork.SubCategories.GetTableNoTracking()
            .Where(sc => !sc.IsDeleted && sc.IsActive && sc.CategoryId == categoryId)
            .OrderBy(sc => sc.Name)
            .Select(sc => new BaseLookupDto
            {
                Id = sc.Id,
                Name = sc.Name
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<List<BaseLookupDto>> GetBrandsAsync(CancellationToken cancellationToken = default)
    {
        return await unitOfWork.Brands.GetTableNoTracking()
            .Where(b => !b.IsDeleted && b.IsActive)
            .OrderBy(b => b.Name)
            .Select(b => new BaseLookupDto { Id = b.Id, Name = b.Name })
            .ToListAsync(cancellationToken);
    }

    public async Task<List<BaseLookupDto>> GetUnitOfMeasuresAsync(CancellationToken cancellationToken = default)
    {
        return await unitOfWork.UnitOfMeasures.GetTableNoTracking()
            .Where(u => !u.IsDeleted && u.IsActive)
            .OrderBy(u => u.Name)
            .Select(u => new BaseLookupDto
            {
                Id = u.Id,
                Name = u.Name
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<List<BaseLookupDto>> GetWarrantiesAsync(CancellationToken cancellationToken = default)
    {
        return await unitOfWork.Warranties.GetTableNoTracking()
            .Where(w => !w.IsDeleted && w.IsActive)
            .OrderBy(w => w.Name)
            .Select(w => new BaseLookupDto
            {
                Id = w.Id,
                Name = w.Name
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<List<BaseLookupDto>> GetVariantAttributesAsync(CancellationToken cancellationToken = default)
    {
        return await unitOfWork.VariantAttributes.GetTableNoTracking()
            .Where(va => !va.IsDeleted && va.IsActive)
            .OrderBy(va => va.Name)
            .Select(va => new BaseLookupDto { Id = va.Id, Name = va.Name })
            .ToListAsync(cancellationToken);
    }

    public async Task<List<RoleLookupDto>> GetRolesAsync(CancellationToken cancellationToken = default)
    {
        return new List<RoleLookupDto>
        {
            new RoleLookupDto { Name = "SuperAdmin", DisplayName = "Super Admin" },
            new RoleLookupDto { Name = "Admin", DisplayName = "Admin" },
            new RoleLookupDto { Name = "Vendor", DisplayName = "Vendor" },
            new RoleLookupDto { Name = "Customer", DisplayName = "Customer" }
        };
    }

    public async Task<List<BaseLookupDto>> GetTagsAsync(CancellationToken cancellationToken = default)
    {
        return await unitOfWork.Tags.GetTableNoTracking()
            .Where(t => t.IsActive)
            .OrderBy(t => t.Name)
            .Select(t => new BaseLookupDto { Id = t.Id, Name = t.Name })
            .ToListAsync(cancellationToken);
    }

    public Task<List<EnumLookupDto>> GetProductPublishStatusesAsync(CancellationToken cancellationToken = default)
    {
        var statuses = Enum.GetValues<ProductPublishStatus>()
            .Select(v => new EnumLookupDto { Id = (int)v, Name = v.ToString() })
            .ToList();

        return Task.FromResult(statuses);
    }

    public Task<List<EnumLookupDto>> GetProductVisibilitiesAsync(CancellationToken cancellationToken = default)
    {
        var visibilities = Enum.GetValues<ProductVisibility>()
            .Select(v => new EnumLookupDto { Id = (int)v, Name = v.ToString() })
            .ToList();

        return Task.FromResult(visibilities);
    }

    public Task<List<EnumLookupDto>> GetProductTypesAsync(CancellationToken cancellationToken = default)
    {
        var types = Enum.GetValues<ProductType>()
            .Select(v => new EnumLookupDto { Id = (int)v, Name = v.ToString() })
            .ToList();

        return Task.FromResult(types);
    }

    public Task<List<EnumLookupDto>> GetSellingTypesAsync(CancellationToken cancellationToken = default)
    {
        var types = Enum.GetValues<SellingType>()
            .Select(v => new EnumLookupDto { Id = (int)v, Name = v.ToString() })
            .ToList();

        return Task.FromResult(types);
    }

    public Task<List<EnumLookupDto>> GetTaxTypesAsync(CancellationToken cancellationToken = default)
    {
        var types = Enum.GetValues<TaxType>()
            .Select(v => new EnumLookupDto { Id = (int)v, Name = v.ToString() })
            .ToList();

        return Task.FromResult(types);
    }

    public Task<List<EnumLookupDto>> GetDiscountTypesAsync(CancellationToken cancellationToken = default)
    {
        var types = Enum.GetValues<DiscountType>()
            .Select(v => new EnumLookupDto { Id = (int)v, Name = v.ToString() })
            .ToList();

        return Task.FromResult(types);
    }
}
