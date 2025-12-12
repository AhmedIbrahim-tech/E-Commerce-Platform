using Application.Common.Bases;

namespace Application.Common.Errors;

public static class ProductErrors
{
    public static ApiResponse ProductNotFound()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            Succeeded = false,
            Message = "Product is not found"
        };
    }

    public static ApiResponse DuplicatedProductName()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Another product with the same name already exists"
        };
    }

    public static ApiResponse DuplicatedProductSku()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Another product with the same SKU already exists"
        };
    }

    public static ApiResponse InvalidPrice()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Product price must be greater than zero"
        };
    }

    public static ApiResponse InvalidStockQuantity()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Stock quantity cannot be negative"
        };
    }

    public static ApiResponse InsufficientStock()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Insufficient stock available for this product"
        };
    }

    public static ApiResponse ProductOutOfStock()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Product is currently out of stock"
        };
    }

    public static ApiResponse ProductNotActive()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Product is not active and cannot be purchased"
        };
    }

    public static ApiResponse InvalidCategory()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Invalid product category"
        };
    }

    public static ApiResponse CannotDeleteProductWithOrders()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Cannot delete product that has associated orders"
        };
    }

    public static ApiResponse FailedToUploadImage()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Failed to upload product image"
        };
    }

    public static ApiResponse ImageRequired()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Product image is required"
        };
    }
}

