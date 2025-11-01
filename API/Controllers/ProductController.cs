using Core.Features.Products.Commands.Models;
using Core.Features.Products.Queries.Models;
using Microsoft.Extensions.Caching.Memory;

namespace API.Controllers;

[Authorize]
public class ProductController(IMemoryCache memoryCache) : AppControllerBase
{
    [AllowAnonymous]
    [HttpGet(Router.ProductRouting.Paginated)]
    public async Task<IActionResult> GetProductPaginatedList([FromQuery] GetProductPaginatedListQuery query)
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

    [Authorize(Roles = "Admin, Employee")]
    [HttpPost(Router.ProductRouting.Create)]
    public async Task<IActionResult> CreateProduct([FromForm] AddProductCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    [Authorize(Roles = "Admin, Employee")]
    [HttpPut(Router.ProductRouting.Edit)]
    public async Task<IActionResult> EditProduct([FromBody] EditProductCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }

    [Authorize(Roles = "Admin, Employee")]
    [HttpDelete(Router.ProductRouting.Delete)]
    public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
    {
        return NewResult(await Mediator.Send(new DeleteProductCommand(id)));
    }

    /// <summary>
    /// Example endpoint demonstrating IMemoryCache usage
    /// </summary>
    [AllowAnonymous]
    [HttpPost("cache/example")]
    public async Task<IActionResult> CacheExample([FromBody] CacheExampleRequest request)
    {
        try
        {
            var cacheKey = $"example:{request.Key}";
            var cacheValue = JsonSerializer.Serialize(request.Value);

            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(request.ExpirationMinutes ?? 30)
            };

            // Set the cached value
            memoryCache.Set(cacheKey, cacheValue, options);

            return Ok(new { message = "Value cached successfully", key = request.Key });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Example endpoint to retrieve a cached value
    /// </summary>
    [AllowAnonymous]
    [HttpGet("cache/example/{key}")]
    public async Task<IActionResult> GetCachedExample(string key)
    {
        try
        {
            var cacheKey = $"example:{key}";
            
            if (!memoryCache.TryGetValue(cacheKey, out string? cached))
            {
                return NotFound(new { message = "Key not found in cache", key });
            }

            var value = JsonSerializer.Deserialize<object>(cached ?? "");

            return Ok(new { key, value });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    public class CacheExampleRequest
    {
        public string Key { get; set; } = string.Empty;
        public object Value { get; set; } = new();
        public int? ExpirationMinutes { get; set; }
    }
}
