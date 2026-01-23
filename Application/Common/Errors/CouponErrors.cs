using Application.Common.Bases;

namespace Application.Common.Errors;

public static class CouponErrors
{
    public static ApiResponse CouponNotFound()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            Succeeded = false,
            Message = "Coupon is not found"
        };
    }

    public static ApiResponse DuplicatedCouponCode()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Another coupon with the same code already exists"
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

    public static ApiResponse CouponExpired()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Coupon has expired"
        };
    }

    public static ApiResponse CouponUsageLimitReached()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Coupon usage limit has been reached"
        };
    }
}
