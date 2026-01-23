using Application.Common.Bases;

namespace Application.Common.Errors;

public static class SubCategoryErrors
{
    public static ApiResponse SubCategoryNotFound()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            Succeeded = false,
            Message = "Sub category is not found"
        };
    }

    public static ApiResponse DuplicatedSubCategoryName()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Another sub category with the same name already exists"
        };
    }

    public static ApiResponse InvalidCategory()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid category specified"
        };
    }

    public static ApiResponse CannotDeleteSubCategoryWithProducts()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Cannot delete sub category that contains products"
        };
    }
}
