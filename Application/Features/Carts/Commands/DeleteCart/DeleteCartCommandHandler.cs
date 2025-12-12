using Application.Common.Bases;
using Application.Common.Errors;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Features.Carts.Commands.DeleteCart;

public class DeleteCartCommandHandler(IMemoryCache memoryCache) : ApiResponseHandler(),
    IRequestHandler<DeleteCartCommand, ApiResponse<string>>
{
    public async Task<ApiResponse<string>> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var cartKey = $"cart:{request.CartId}";
            memoryCache.Remove(cartKey);
            return Deleted<string>();
        }
        catch (Exception)
        {
            return new ApiResponse<string>(CartErrors.CannotModifyCart());
        }
    }
}


