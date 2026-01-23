namespace Application.ServicesHandlers.Services;

public interface ILookUpsService
{
    Task<List<BaseLookupDto>> GetCategoriesAsync(CancellationToken cancellationToken = default);
    Task<List<BaseLookupDto>> GetSubCategoriesAsync(CancellationToken cancellationToken = default);
    Task<List<BaseLookupDto>> GetBrandsAsync(CancellationToken cancellationToken = default);
    Task<List<BaseLookupDto>> GetUnitOfMeasuresAsync(CancellationToken cancellationToken = default);
    Task<List<BaseLookupDto>> GetWarrantiesAsync(CancellationToken cancellationToken = default);
    Task<List<BaseLookupDto>> GetVariantAttributesAsync(CancellationToken cancellationToken = default);
    Task<List<BaseLookupDto>> GetSubCategoriesByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task<List<RoleLookupDto>> GetRolesAsync(CancellationToken cancellationToken = default);

    Task<List<EnumLookupDto>> GetProductPublishStatusesAsync(CancellationToken cancellationToken = default);
    Task<List<EnumLookupDto>> GetProductVisibilitiesAsync(CancellationToken cancellationToken = default);
    Task<List<EnumLookupDto>> GetProductTypesAsync(CancellationToken cancellationToken = default);
    Task<List<EnumLookupDto>> GetSellingTypesAsync(CancellationToken cancellationToken = default);
    Task<List<EnumLookupDto>> GetTaxTypesAsync(CancellationToken cancellationToken = default);
    Task<List<EnumLookupDto>> GetDiscountTypesAsync(CancellationToken cancellationToken = default);

    Task<List<BaseLookupDto>> GetTagsAsync(CancellationToken cancellationToken = default);
}
