using System.Net;
using System.Text.Json;
using Government.API.Exceptions;
using AppException = Government.API.Exceptions.AppException;

namespace Government.API.Middleware
{
    /// <summary>
    /// Global exception handling middleware for consistent error responses
    /// </summary>
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse();

            switch (exception)
            {
                case ValidationException valEx:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.ErrorCode = valEx.ErrorCode;
                    response.Message = valEx.Message;
                    response.ValidationErrors = valEx.ValidationErrors.Cast<object>().ToList();
                    _logger.LogWarning($"Validation failed: {valEx.Message}");
                    break;

                case AppException appEx:
                    context.Response.StatusCode = appEx.HttpStatusCode ?? 500;
                    response.ErrorCode = appEx.ErrorCode;
                    response.Message = appEx.Message;
                    response.AdditionalData = appEx.AdditionalData;
                    _logger.LogWarning(appEx, $"Application exception: {appEx.ErrorCode}");
                    break;

                case ArgumentNullException argNullEx:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.ErrorCode = "ARGUMENT_NULL";
                    response.Message = $"Required argument is null: {argNullEx.ParamName}";
                    _logger.LogWarning(argNullEx, "Argument null exception");
                    break;

                case ArgumentException argEx:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.ErrorCode = "INVALID_ARGUMENT";
                    response.Message = argEx.Message;
                    _logger.LogWarning(argEx, "Argument exception");
                    break;

                case KeyNotFoundException keyEx:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.ErrorCode = "RESOURCE_NOT_FOUND";
                    response.Message = keyEx.Message;
                    _logger.LogWarning(keyEx, "Key not found");
                    break;

                case UnauthorizedAccessException unAuthEx:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.ErrorCode = "UNAUTHORIZED_ACCESS";
                    response.Message = "You are not authorized to perform this action";
                    _logger.LogWarning(unAuthEx, "Unauthorized access attempt");
                    break;

                case OperationCanceledException opEx:
                    context.Response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                    response.ErrorCode = "OPERATION_CANCELLED";
                    response.Message = "The operation was cancelled";
                    _logger.LogWarning(opEx, "Operation cancelled");
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.ErrorCode = "INTERNAL_SERVER_ERROR";
                    response.Message = "An unexpected error occurred. Please try again later.";
                    _logger.LogError(exception, "Unhandled exception");
                    break;
            }

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            return context.Response.WriteAsync(jsonResponse);
        }
    }

    /// <summary>
    /// Standard error response model
    /// </summary>
    public class ErrorResponse
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public object AdditionalData { get; set; }
        public List<object> ValidationErrors { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public ErrorResponse()
        {
            ValidationErrors = new List<object>();
        }
    }
}
