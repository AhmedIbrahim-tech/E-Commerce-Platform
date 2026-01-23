using Application.Common.Bases;

namespace Application.Common.Errors;

public static class DiscountErrors
{
    public static ApiResponse DiscountNotFound()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            Succeeded = false,
            Message = "Discount is not found"
        };
    }

    public static ApiResponse DuplicatedDiscountName()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Another discount with the same name already exists"
        };
    }

    public static ApiResponse InvalidDateRange()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "End date must be after start date"
        };
    }
}
