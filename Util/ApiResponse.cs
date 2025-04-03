using System.Text.Json.Serialization;

namespace TerracoDaCida.Util
{
    public class ApiResponse<T>
    {
        [JsonPropertyName("StatusCode")]
        public int StatusCode { get; set; }
        [JsonPropertyName("data")]
        public T? Data { get; set; } = default(T?);
        [JsonPropertyName("errorMessage")]
        public string? ErrorMessage { get; set; }

        public static ApiResponse<T> Error(string errorMessage, int statusCode = StatusCodes.Status400BadRequest)
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                ErrorMessage = errorMessage,
            };
        }

        public static ApiResponse<T> SuccessOk(T data)
        {
            return new ApiResponse<T>
            {
                StatusCode = StatusCodes.Status200OK,
                Data = data
            };
        }

        public static ApiResponse<T> SuccessOk(T data, int statusCode, string? errorMessage)
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                Data = data,
                ErrorMessage = errorMessage
            };
        }

        public static ApiResponse<T> SuccessCreated(T data)
        {
            return new ApiResponse<T>
            {
                StatusCode = StatusCodes.Status201Created,
                Data = data
            };
        }

        public static ApiResponse<T> NotFound(T data)
        {
            return new ApiResponse<T>
            {
                StatusCode = StatusCodes.Status404NotFound,
                Data = data
            };
        }

        public static ApiResponse<T> NotFound(T data, string errorMessage)
        {
            return new ApiResponse<T>
            {
                StatusCode = StatusCodes.Status404NotFound,
                Data = data,
                ErrorMessage = errorMessage
            };
        }

        public static ApiResponse<T> BadRequest(T data, string errorMessage)
        {
            return new ApiResponse<T>
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Data = data,
                ErrorMessage = errorMessage
            };
        }

        public static ApiResponse<T> Forbidden(T data, string errorMessage)
        {
            return new ApiResponse<T>
            {
                StatusCode = StatusCodes.Status403Forbidden,
                Data = data,
                ErrorMessage = errorMessage
            };
        }

        public static ApiResponse<T> UnprocessableEntity(T data, string errorMessage)
        {
            return new ApiResponse<T>
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity,
                Data = data,
                ErrorMessage = errorMessage
            };
        }

        public static ApiResponse<T> Accepted(T data, string errorMessage)
        {
            return new ApiResponse<T>
            {
                StatusCode = StatusCodes.Status202Accepted,
                Data = data,
                ErrorMessage = errorMessage
            };
        }

        public static ApiResponse<T> NoContent(T data)
        {
            return new ApiResponse<T>
            {
                StatusCode = StatusCodes.Status204NoContent,
                Data = data
            };
        }

        public static ApiResponse<T> NoContent(T data, string errorMessage)
        {
            return new ApiResponse<T>
            {
                StatusCode = StatusCodes.Status204NoContent,
                Data = data,
                ErrorMessage = errorMessage
            };
        }
    }
}
