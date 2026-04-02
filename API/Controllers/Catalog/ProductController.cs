using Application.Features.Products.Commands.AddProduct;
using Application.Features.Products.Commands.DeleteProduct;
using Application.Features.Products.Commands.EditProduct;
using Application.Features.Products.Queries.GetProductById;
using Application.Features.Products.Queries.GetProductPaginatedList;
using Infrastructure.Data.Authorization;

namespace API.Controllers.Catalog;

[Authorize]
public class ProductController : AppControllerBase
{
    [AllowAnonymous]
    [HttpPost(Router.ProductRouting.Paginated)]
    public async Task<IActionResult> GetProductPaginatedList([FromBody] GetProductPaginatedListQuery query)
    {
        var response = await Mediator.Send(query);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet(Router.ProductRouting.GetSingle)]
    public async Task<IActionResult> GetProductById([FromQuery] GetProductByIdQuery query)
    {
        return NewResult(await Mediator.Send(query));
    }

    [Authorize(Roles = Roles.Admin, Policy = Policies.Product.Create)]
    [HttpPost(Router.ProductRouting.Create)]
    public async Task<IActionResult> CreateProduct([FromForm] AddProductCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    [Authorize(Roles = Roles.Admin, Policy = Policies.Product.Edit)]
    [HttpPut(Router.ProductRouting.Edit)]
    public async Task<IActionResult> EditProduct([FromForm] EditProductCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    [Authorize(Roles = Roles.Admin, Policy = Policies.Product.Delete)]
    [HttpDelete(Router.ProductRouting.Delete)]
    public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
    {
        return NewResult(await Mediator.Send(new DeleteProductCommand(id)));
    }
}
