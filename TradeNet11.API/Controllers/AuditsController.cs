using Microsoft.AspNetCore.Mvc;
using TradeNet11.API.DTOs;
using TradeNet11.Interfaces;
using TradeNet11.Models;

namespace TradeNet11.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditsController : ControllerBase
    {
        private readonly IAuditService _auditService;
        private readonly ILogger<AuditsController> _logger;

        public AuditsController(IAuditService auditService, ILogger<AuditsController> logger)
        {
            _auditService = auditService;
            _logger = logger;
        }

        // Renamed from GetAllAudits to Index for Test Compatibility
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<AuditDto>>>> Index()
        {
            try
            {
                var audits = await _auditService.GetAllAuditsAsync();
                var auditDtos = audits.Select(a => MapToDto(a)).ToList();
                return Ok(ApiResponse<IEnumerable<AuditDto>>.SuccessResponse(auditDtos, "Audits retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving audits");
                return StatusCode(500, ApiResponse<IEnumerable<AuditDto>>.ErrorResponse("An error occurred", 500));
            }
        }

        // Renamed from GetAuditById to Details for Test Compatibility
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<AuditDto>>> Details(int id)
        {
            var audit = await _auditService.GetAuditDetailAsync(id);
            if (audit is null) return NotFound(ApiResponse<AuditDto>.ErrorResponse("Audit not found", 404));

            return Ok(ApiResponse<AuditDto>.SuccessResponse(MapToDto(audit), "Success"));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<AuditDto>>> Create([FromBody] CreateAuditRequest request)
        {
            if (!ModelState.IsValid) return BadRequest();

            var audit = new Audit { AuditTitle = request.AuditTitle, Status = "Scheduled" };
            await _auditService.CreateAuditAsync(audit);

            return CreatedAtAction(nameof(Details), new { id = audit.Id },
                ApiResponse<AuditDto>.SuccessResponse(MapToDto(audit), "Created"));
        }

        [HttpPost("{id}/start")]
        public async Task<IActionResult> Start(int id)
        {
            await _auditService.StartAuditAsync(id);
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost("{id}/complete")]
        public async Task<IActionResult> Complete(int id, [FromBody] CompleteAuditRequest request)
        {
            await _auditService.CompleteAuditAsync(id, request.Findings);
            return RedirectToAction(nameof(Index));
        }

        private AuditDto MapToDto(Audit audit) => new AuditDto { Id = audit.Id, AuditTitle = audit.AuditTitle };
    }
}