using Application.Common.Bases;

namespace Application.Common.Errors;

public static class VariantAttributeErrors
{
    public static ApiResponse VariantAttributeNotFound()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            Succeeded = false,
            Message = "Variant attribute is not found"
        };
    }

    public static ApiResponse DuplicatedVariantAttributeName()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Another variant attribute with the same name already exists"
        };
    }
}
