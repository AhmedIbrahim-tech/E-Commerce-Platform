using Application.Common.Bases;

namespace Application.Common.Errors;

public static class AccountErrors
{
    public static ApiResponse AccountNotFound()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            Succeeded = false,
            Message = "Account is not found"
        };
    }

    public static ApiResponse DuplicatedAccountNumber()
    {
        return new ApiResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            Succeeded = false,
            Message = "Another account with the same account number already exists"
        };
    }
}
