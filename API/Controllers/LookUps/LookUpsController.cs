using Application.Common.DTOs;
using Application.ServicesHandlers.Services;
using Infrastructure.Data.Authorization;

namespace API.Controllers.LookUps;

[Authorize]
public class LookUpsController(ILookUpsService lookUpsService) : AppControllerBase
{
    [AllowAnonymous]
    [HttpGet(Router.LookUpsRouting.Categories)]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken) => Ok(await lookUpsService.GetCategoriesAsync(cancellationToken));

    [AllowAnonymous]
    [HttpGet(Router.LookUpsRouting.SubCategories)]
    public async Task<IActionResult> GetSubCategories(CancellationToken cancellationToken) => Ok(await lookUpsService.GetSubCategoriesAsync(cancellationToken));

    [AllowAnonymous]
    [HttpPost(Router.LookUpsRouting.SubCategoriesByCategory)]
    public async Task<IActionResult> GetSubCategoriesByCategory([FromBody] GetSubCategoriesByCategoryRequest request, CancellationToken cancellationToken) => Ok(await lookUpsService.GetSubCategoriesByCategoryIdAsync(request.CategoryId, cancellationToken));

    [AllowAnonymous]
    [HttpGet(Router.LookUpsRouting.Brands)]
    public async Task<IActionResult> GetBrands(CancellationToken cancellationToken) => Ok(await lookUpsService.GetBrandsAsync(cancellationToken));

    [AllowAnonymous]
    [HttpGet(Router.LookUpsRouting.UnitOfMeasures)]
    public async Task<IActionResult> GetUnitOfMeasures(CancellationToken cancellationToken) => Ok(await lookUpsService.GetUnitOfMeasuresAsync(cancellationToken));

    [AllowAnonymous]
    [HttpGet(Router.LookUpsRouting.Warranties)]
    public async Task<IActionResult> GetWarranties(CancellationToken cancellationToken) => Ok(await lookUpsService.GetWarrantiesAsync(cancellationToken));

    [AllowAnonymous]
    [HttpGet(Router.LookUpsRouting.VariantAttributes)]
    public async Task<IActionResult> GetVariantAttributes(CancellationToken cancellationToken) => Ok(await lookUpsService.GetVariantAttributesAsync(cancellationToken));

    [AllowAnonymous]
    [HttpGet(Router.LookUpsRouting.Roles)]
    public async Task<IActionResult> GetRoles(CancellationToken cancellationToken) => Ok(await lookUpsService.GetRolesAsync(cancellationToken));

    [AllowAnonymous]
    [HttpGet(Router.LookUpsRouting.ProductPublishStatuses)]
    public async Task<IActionResult> GetProductPublishStatuses(CancellationToken cancellationToken) => Ok(await lookUpsService.GetProductPublishStatusesAsync(cancellationToken));

    [AllowAnonymous]
    [HttpGet(Router.LookUpsRouting.ProductVisibilities)]
    public async Task<IActionResult> GetProductVisibilities(CancellationToken cancellationToken) => Ok(await lookUpsService.GetProductVisibilitiesAsync(cancellationToken));

    [AllowAnonymous]
    [HttpGet(Router.LookUpsRouting.ProductTypes)]
    public async Task<IActionResult> GetProductTypes(CancellationToken cancellationToken) => Ok(await lookUpsService.GetProductTypesAsync(cancellationToken));

    [AllowAnonymous]
    [HttpGet(Router.LookUpsRouting.SellingTypes)]
    public async Task<IActionResult> GetSellingTypes(CancellationToken cancellationToken) => Ok(await lookUpsService.GetSellingTypesAsync(cancellationToken));

    [AllowAnonymous]
    [HttpGet(Router.LookUpsRouting.TaxTypes)]
    public async Task<IActionResult> GetTaxTypes(CancellationToken cancellationToken) => Ok(await lookUpsService.GetTaxTypesAsync(cancellationToken));

    [AllowAnonymous]
    [HttpGet(Router.LookUpsRouting.DiscountTypes)]
    public async Task<IActionResult> GetDiscountTypes(CancellationToken cancellationToken) => Ok(await lookUpsService.GetDiscountTypesAsync(cancellationToken));

    [AllowAnonymous]
    [HttpGet(Router.LookUpsRouting.Tags)]
    public async Task<IActionResult> GetTags(CancellationToken cancellationToken) => Ok(await lookUpsService.GetTagsAsync(cancellationToken));
}
