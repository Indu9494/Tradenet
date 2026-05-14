using Tradenet_ProgramManager_2.API.Core;

namespace Tradenet_ProgramManager_2.API.Middleware
{
    /// <summary>
    /// Middleware to register and initialize all modules
    /// </summary>
    public class ModuleInitializationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ModuleInitializationMiddleware> _logger;

        public ModuleInitializationMiddleware(RequestDelegate next, ILogger<ModuleInitializationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IInterModuleService interModuleService)
        {
            // Add module information to response headers
            context.Response.Headers.Add("X-API-Type", "Unified-Microservices");
            context.Response.Headers.Add("X-API-Version", "1.0.0");

            await _next(context);
        }
    }
}
