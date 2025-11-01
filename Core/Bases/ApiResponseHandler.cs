namespace Core.Bases
{
    public class ApiResponseHandler
    {
        public ApiResponseHandler()
        {
        }
        public ApiResponse<T> Success<T>(T entity, object Meta = null)
        {
            return new ApiResponse<T>()
            {
                Data = entity,
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = "Success",
                Meta = Meta
            };
        }
        public ApiResponse<T> Created<T>(T entity, object Meta = null)
        {
            return new ApiResponse<T>()
            {
                Data = entity,
                StatusCode = System.Net.HttpStatusCode.Created,
                Succeeded = true,
                Message = "Created",
                Meta = Meta
            };
        }
        public ApiResponse<T> Edit<T>(T entity, object Meta = null)
        {
            return new ApiResponse<T>()
            {
                Data = entity,
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = "Updated",
                Meta = Meta
            };
        }
        public ApiResponse<T> Deleted<T>(string Message = null)
        {
            return new ApiResponse<T>()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = Message ?? "Deleted"
            };
        }
        public ApiResponse<T> NotFound<T>(string message = null)
        {
            return new ApiResponse<T>()
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                Succeeded = false,
                Message = message ?? "NotFound"
            };
        }
        public ApiResponse<T> Unauthorized<T>(string Message = null)
        {
            return new ApiResponse<T>()
            {
                StatusCode = System.Net.HttpStatusCode.Unauthorized,
                Succeeded = true,
                Message = Message ?? "Unauthorized"
            };
        }
        public ApiResponse<T> BadRequest<T>(string Message = null)
        {
            return new ApiResponse<T>()
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Succeeded = false,
                Message = Message ?? "BadRequest"
            };
        }
        public ApiResponse<T> UnprocessableEntity<T>(string Message = null)
        {
            return new ApiResponse<T>()
            {
                StatusCode = System.Net.HttpStatusCode.UnprocessableEntity,
                Succeeded = false,
                Message = Message ?? "UnprocessableEntity"
            };
        }


    }
}
