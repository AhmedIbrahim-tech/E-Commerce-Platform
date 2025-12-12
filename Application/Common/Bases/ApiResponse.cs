namespace Application.Common.Bases;

public class ApiResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public object? Meta { get; set; }
    public bool Succeeded { get; set; }
    public string? Message { get; set; }
    public List<string>? Errors { get; set; }

    public ApiResponse()
    {
    }

    public ApiResponse(string? message, bool succeeded = false)
    {
        Message = message;
        Succeeded = succeeded;
    }

    public ApiResponse(HttpStatusCode statusCode, string? message, bool succeeded = false)
    {
        StatusCode = statusCode;
        Message = message;
        Succeeded = succeeded;
    }
}

public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; set; }

    public ApiResponse()
    {
    }

    public ApiResponse(T data, string? message = null, object? meta = null)
        : base(HttpStatusCode.OK, message, true)
    {
        Data = data;
        Meta = meta;
    }

    public ApiResponse(T data, HttpStatusCode statusCode, string? message = null, object? meta = null)
        : base(statusCode, message, true)
    {
        Data = data;
        Meta = meta;
    }

    public ApiResponse(ApiResponse errorResponse)
        : base(errorResponse.StatusCode, errorResponse.Message, errorResponse.Succeeded)
    {
        Meta = errorResponse.Meta;
        Errors = errorResponse.Errors;
    }

    public static ApiResponse<T> FromError(ApiResponse errorResponse)
    {
        return new ApiResponse<T>(errorResponse);
    }
}
