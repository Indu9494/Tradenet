using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace Tradenet_ProgramManager_2.API.Middleware
{
    /// <summary>
    /// Global exception handler using .NET 8/10 IExceptionHandler interface for production-grade error handling.
    /// This handler catches all unhandled exceptions, logs them using ILogger, and returns standardized JSON responses.
    /// </summary>
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An unhandled exception occurred. TraceId: {TraceId}", context.TraceIdentifier);

            context.Response.ContentType = "application/json";

            var response = new ErrorResponse
            {
                Success = false,
                Message = "An internal server error occurred. Please contact support.",
                TraceId = context.TraceIdentifier,
                ErrorCode = GetErrorCode(exception)
            };

            // Return 500 for all unhandled exceptions
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            // Only include detailed error information in Development environment
            if (context.RequestServices.GetRequiredService<IHostEnvironment>().IsDevelopment())
            {
                response.Message = exception.Message;
                response.Details = exception.ToString();
            }

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            await context.Response.WriteAsync(json, cancellationToken);

            return true;
        }

        private static string GetErrorCode(Exception exception)
        {
            return exception switch
            {
                ArgumentNullException => "INVALID_ARGUMENT",
                InvalidOperationException => "INVALID_OPERATION",
                UnauthorizedAccessException => "UNAUTHORIZED",
                _ => "INTERNAL_SERVER_ERROR"
            };
        }
    }

    /// <summary>
    /// Standard error response model for API responses
    /// </summary>
    public class ErrorResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? TraceId { get; set; }
        public string ErrorCode { get; set; } = "INTERNAL_SERVER_ERROR";
        public string? Details { get; set; }
    }
}
