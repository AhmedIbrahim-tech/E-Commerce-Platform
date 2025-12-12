using Application.Common.Bases;

namespace Application.Common.Errors;

public static class CategoryErrors
{
    public static ApiResponse CategoryNotFound()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            Succeeded = false,
            Message = "Category is not found"
        };
    }

    public static ApiResponse DuplicatedCategoryName()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Another category with the same name already exists"
        };
    }

    public static ApiResponse InvalidParentCategory()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid parent category specified"
        };
    }

    public static ApiResponse CircularCategoryReference()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Cannot set category as its own parent or create circular references"
        };
    }

    public static ApiResponse CannotDeleteCategoryWithProducts()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Cannot delete category that contains products"
        };
    }

    public static ApiResponse CannotDeleteCategoryWithSubcategories()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Cannot delete category that has subcategories"
        };
    }

    public static ApiResponse CategoryNotActive()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Category is not active"
        };
    }

    public static ApiResponse InvalidCategorySlug()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid category slug format"
        };
    }

    public static ApiResponse DuplicatedCategorySlug()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Another category with the same slug already exists"
        };
    }

    public static ApiResponse CategoryDepthExceeded()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Maximum category depth has been exceeded"
        };
    }
}

