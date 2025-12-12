namespace Application.Common.Bases;

public class ApiResponseHandler
{
    public ApiResponseHandler()
    {
    }

    public ApiResponse<T> Success<T>(T entity, object? meta = null)
    {
        return new ApiResponse<T>(entity, HttpStatusCode.OK, "Success", meta);
    }

    public ApiResponse<T> Success<T>(T entity, string message, object? meta = null)
    {
        return new ApiResponse<T>(entity, HttpStatusCode.OK, message, meta);
    }

    public ApiResponse<T> Created<T>(T entity, object? meta = null)
    {
        return new ApiResponse<T>(entity, HttpStatusCode.Created, "Created", meta);
    }

    public ApiResponse<T> Created<T>(T entity, string message, object? meta = null)
    {
        return new ApiResponse<T>(entity, HttpStatusCode.Created, message, meta);
    }

    public ApiResponse<T> Edit<T>(T entity, object? meta = null)
    {
        return new ApiResponse<T>(entity, HttpStatusCode.OK, "Updated", meta);
    }

    public ApiResponse<T> Edit<T>(T entity, string message, object? meta = null)
    {
        return new ApiResponse<T>(entity, HttpStatusCode.OK, message, meta);
    }

    public ApiResponse<T> Deleted<T>(string? message = null)
    {
        return new ApiResponse<T>(default(T)!, HttpStatusCode.OK, message ?? "Deleted");
    }

    public ApiResponse<T> NotFound<T>(string? message = null)
    {
        return new ApiResponse<T>(new ApiResponse(HttpStatusCode.NotFound, message ?? "NotFound", false));
    }

    public ApiResponse<T> Unauthorized<T>(string? message = null)
    {
        return new ApiResponse<T>(new ApiResponse(HttpStatusCode.Unauthorized, message ?? "Unauthorized", false));
    }

    public ApiResponse<T> BadRequest<T>(string? message = null)
    {
        return new ApiResponse<T>(new ApiResponse(HttpStatusCode.BadRequest, message ?? "BadRequest", false));
    }

    public ApiResponse<T> UnprocessableEntity<T>(string? message = null)
    {
        return new ApiResponse<T>(new ApiResponse(HttpStatusCode.UnprocessableEntity, message ?? "UnprocessableEntity", false));
    }

    public ApiResponse<T> Error<T>(ApiResponse errorResponse)
    {
        return new ApiResponse<T>(errorResponse);
    }
}
