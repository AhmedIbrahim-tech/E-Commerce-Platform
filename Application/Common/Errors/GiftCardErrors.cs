using Application.Common.Bases;

namespace Application.Common.Errors;

public static class GiftCardErrors
{
    public static ApiResponse GiftCardNotFound()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            Succeeded = false,
            Message = "Gift card is not found"
        };
    }

    public static ApiResponse DuplicatedGiftCardCode()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Another gift card with the same code already exists"
        };
    }

    public static ApiResponse GiftCardExpired()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Gift card has expired"
        };
    }

    public static ApiResponse GiftCardAlreadyRedeemed()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Gift card has already been redeemed"
        };
    }

    public static ApiResponse InsufficientBalance()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = "Gift card has insufficient balance"
        };
    }
}
