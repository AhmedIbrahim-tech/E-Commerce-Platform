namespace Application.Common.Bases;

public class ApiResponseHandler
{
    public ApiResponseHandler()
    {
    }

    public static ApiResponse<T>Success<T>(T entity, object? meta = null)
    {
        return new ApiResponse<T>(entity, HttpStatusCode.OK, "Success", meta);
    }

    public static ApiResponse<T>Success<T>(T entity, string message, object? meta = null)
    {
        return new ApiResponse<T>(entity, HttpStatusCode.OK, message, meta);
    }

    public static ApiResponse<T>Created<T>(T entity, object? meta = null)
    {
        return new ApiResponse<T>(entity, HttpStatusCode.Created, "Created", meta);
    }

    public static ApiResponse<T>Created<T>(T entity, string message, object? meta = null)
    {
        return new ApiResponse<T>(entity, HttpStatusCode.Created, message, meta);
    }

    public static ApiResponse<T>Edit<T>(T entity, object? meta = null)
    {
        return new ApiResponse<T>(entity, HttpStatusCode.OK, "Updated", meta);
    }

    public static ApiResponse<T>Edit<T>(T entity, string message, object? meta = null)
    {
        return new ApiResponse<T>(entity, HttpStatusCode.OK, message, meta);
    }

    public static ApiResponse<T>Deleted<T>(string? message = null)
    {
        return new ApiResponse<T>(default(T)!, HttpStatusCode.OK, message ?? "Deleted");
    }

    public static ApiResponse<T>NotFound<T>(string? message = null)
    {
        return new ApiResponse<T>(new ApiResponse(HttpStatusCode.NotFound, message ?? "NotFound", false));
    }

    public static ApiResponse<T>Unauthorized<T>(string? message = null)
    {
        return new ApiResponse<T>(new ApiResponse(HttpStatusCode.Unauthorized, message ?? "Unauthorized", false));
    }

    public static ApiResponse<T>Forbidden<T>(string? message = null)
    {
        return new ApiResponse<T>(new ApiResponse(HttpStatusCode.Forbidden, message ?? "Forbidden", false));
    }

    public static ApiResponse<T>BadRequest<T>(string? message = null)
    {
        return new ApiResponse<T>(new ApiResponse(HttpStatusCode.BadRequest, message ?? "BadRequest", false));
    }

    public static ApiResponse<T>UnprocessableEntity<T>(string? message = null)
    {
        return new ApiResponse<T>(new ApiResponse(HttpStatusCode.UnprocessableEntity, message ?? "UnprocessableEntity", false));
    }

    public static ApiResponse<T>Error<T>(ApiResponse errorResponse)
    {
        return new ApiResponse<T>(errorResponse);
    }
}
