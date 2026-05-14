using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tradenet_ProgramManager_2.API.Core;

namespace Tradenet_ProgramManager_2.API.Controllers
{
    /// <summary>
    /// API Orchestration Controller
    /// Handles cross-module operations and queries
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrchestratorController : ControllerBase
    {
        private readonly IInterModuleService _interModuleService;
        private readonly ILogger<OrchestratorController> _logger;

        public OrchestratorController(IInterModuleService interModuleService, ILogger<OrchestratorController> logger)
        {
            _interModuleService = interModuleService;
            _logger = logger;
        }

        /// <summary>
        /// Get data from a specific module
        /// </summary>
        [HttpGet("module/{moduleName}/{endpoint}")]
        public async Task<IActionResult> GetModuleData(string moduleName, string endpoint, [FromQuery] string? id = null)
        {
            _logger.LogInformation("Fetching data from module {ModuleName}, endpoint {Endpoint}", moduleName, endpoint);

            if (!_interModuleService.IsModuleRegistered(moduleName))
            {
                return NotFound(new { message = $"Module {moduleName} not found" });
            }

            var data = await _interModuleService.GetModuleDataAsync<object>(moduleName, endpoint, id);
            if (data == null)
            {
                return NotFound(new { message = $"No data found from module {moduleName}" });
            }

            return Ok(data);
        }

        /// <summary>
        /// Send data to a module
        /// </summary>
        [HttpPost("module/{moduleName}/{endpoint}")]
        public async Task<IActionResult> SendModuleData(string moduleName, string endpoint, [FromBody] object data)
        {
            _logger.LogInformation("Sending data to module {ModuleName}, endpoint {Endpoint}", moduleName, endpoint);

            if (!_interModuleService.IsModuleRegistered(moduleName))
            {
                return NotFound(new { message = $"Module {moduleName} not found" });
            }

            var result = await _interModuleService.SendModuleDataAsync<object, object>(moduleName, endpoint, data);
            return Ok(result);
        }

        /// <summary>
        /// Get health status of all modules
        /// </summary>
        [HttpGet("health")]
        [AllowAnonymous]
        public IActionResult GetHealth()
        {
            return Ok(new
            {
                status = "healthy",
                timestamp = DateTime.UtcNow,
                message = "Unified API is running"
            });
        }
    }
}
