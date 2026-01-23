using Application.Common.Bases;

namespace Application.Common.Errors;

public static class BrandErrors
{
    public static ApiResponse BrandNotFound()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            Succeeded = false,
            Message = "Brand is not found"
        };
    }

    public static ApiResponse DuplicatedBrandName()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Another brand with the same name already exists"
        };
    }

    public static ApiResponse CannotDeleteBrandWithProducts()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Cannot delete brand that contains products"
        };
    }
}
