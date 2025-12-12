using Application.Common.Bases;

namespace Application.Common.Errors;

public static class ReviewErrors
{
    public static ApiResponse ReviewNotFound()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            Succeeded = false,
            Message = "Review is not found"
        };
    }

    public static ApiResponse DuplicatedReview()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "You have already reviewed this product"
        };
    }

    public static ApiResponse InvalidRating()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid rating value. Rating must be between 1 and 5"
        };
    }

    public static ApiResponse CannotReviewOwnProduct()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Cannot review your own product"
        };
    }

    public static ApiResponse CannotReviewWithoutPurchase()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "You must purchase this product before you can review it"
        };
    }

    public static ApiResponse ReviewContentTooShort()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Review content is too short. Minimum length is required"
        };
    }

    public static ApiResponse ReviewContentTooLong()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Review content exceeds maximum allowed length"
        };
    }

    public static ApiResponse CannotModifyReview()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Cannot modify this review"
        };
    }

    public static ApiResponse CannotDeleteReview()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Cannot delete this review"
        };
    }

    public static ApiResponse ReviewAlreadyReported()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "This review has already been reported"
        };
    }
}

