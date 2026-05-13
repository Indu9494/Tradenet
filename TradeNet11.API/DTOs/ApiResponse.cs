namespace TradeNet11.API.DTOs
{
    /// <summary>
    /// API Response wrapper for consistent response formatting
    /// </summary>
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public int? StatusCode { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string message = "Operation successful")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = 200
            };
        }

        public static ApiResponse<T> ErrorResponse(string message, int statusCode = 400)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default,
                StatusCode = statusCode
            };
        }
    }

    /// <summary>
    /// Non-generic response wrapper for list operations
    /// </summary>
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int? StatusCode { get; set; }

        public static ApiResponse SuccessResponse(string message = "Operation successful")
        {
            return new ApiResponse
            {
                Success = true,
                Message = message,
                StatusCode = 200
            };
        }

        public static ApiResponse ErrorResponse(string message, int statusCode = 400)
        {
            return new ApiResponse
            {
                Success = false,
                Message = message,
                StatusCode = statusCode
            };
        }
    }
}
