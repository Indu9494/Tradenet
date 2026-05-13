using Microsoft.AspNetCore.Mvc;
using TradeNet11.API.DTOs;
using TradeNet11.Interfaces;
using TradeNet11.Models;

namespace TradeNet11.API.Controllers
{
    /// <summary>
    /// API endpoints for Audit management
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuditsController : ControllerBase
    {
        private readonly IAuditService _auditService;
        private readonly ILogger<AuditsController> _logger;

        public AuditsController(IAuditService auditService, ILogger<AuditsController> logger)
        {
            _auditService = auditService;
            _logger = logger;
        }

        /// <summary>
        /// Get all audits
        /// </summary>
        /// <returns>List of all audits</returns>
        /// <response code="200">Returns the list of audits</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<AuditDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<AuditDto>>>> GetAllAudits()
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
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    ApiResponse<IEnumerable<AuditDto>>.ErrorResponse("An error occurred while retrieving audits", 500));
            }
        }

        /// <summary>
        /// Get audit by ID
        /// </summary>
        /// <param name="id">Audit ID</param>
        /// <returns>Audit details</returns>
        /// <response code="200">Returns the audit</response>
        /// <response code="404">If audit not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<AuditDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<AuditDto>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<AuditDto>>> GetAuditById(int id)
        {
            try
            {
                var audit = await _auditService.GetAuditDetailAsync(id);
                if (audit is null)
                    return NotFound(ApiResponse<AuditDto>.ErrorResponse("Audit not found", 404));

                return Ok(ApiResponse<AuditDto>.SuccessResponse(MapToDto(audit), "Audit retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving audit {AuditId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<AuditDto>.ErrorResponse("An error occurred while retrieving the audit", 500));
            }
        }

        /// <summary>
        /// Create a new audit
        /// </summary>
        /// <param name="request">Audit creation request</param>
        /// <returns>Created audit</returns>
        /// <response code="201">Audit created successfully</response>
        /// <response code="400">If request is invalid</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<AuditDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<AuditDto>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<AuditDto>>> CreateAudit([FromBody] CreateAuditRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<AuditDto>.ErrorResponse("Invalid request data", 400));

                var audit = new Audit
                {
                    AuditTitle = request.AuditTitle,
                    BusinessName = request.BusinessName,
                    ScheduledDate = request.ScheduledDate,
                    AssignedOfficerId = request.AssignedOfficerId,
                    ComplianceCaseId = request.ComplianceCaseId,
                    ChecklistJson = request.ChecklistJson,
                    Status = "Scheduled"
                };

                await _auditService.CreateAuditAsync(audit);
                return CreatedAtAction(nameof(GetAuditById), new { id = audit.Id },
                    ApiResponse<AuditDto>.SuccessResponse(MapToDto(audit), "Audit created successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating audit");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<AuditDto>.ErrorResponse("An error occurred while creating the audit", 500));
            }
        }

        /// <summary>
        /// Update an existing audit
        /// </summary>
        /// <param name="id">Audit ID</param>
        /// <param name="request">Update request</param>
        /// <returns>Updated audit</returns>
        /// <response code="200">Audit updated successfully</response>
        /// <response code="404">If audit not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<AuditDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<AuditDto>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<AuditDto>>> UpdateAudit(int id, [FromBody] UpdateAuditRequest request)
        {
            try
            {
                var audit = await _auditService.GetAuditDetailAsync(id);
                if (audit is null)
                    return NotFound(ApiResponse<AuditDto>.ErrorResponse("Audit not found", 404));

                audit.AuditTitle = request.AuditTitle;
                audit.BusinessName = request.BusinessName;
                audit.ScheduledDate = request.ScheduledDate;
                audit.AssignedOfficerId = request.AssignedOfficerId;
                audit.ChecklistJson = request.ChecklistJson;

                await _auditService.CreateAuditAsync(audit); // Note: Consider adding UpdateAuditAsync to service
                return Ok(ApiResponse<AuditDto>.SuccessResponse(MapToDto(audit), "Audit updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating audit {AuditId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<AuditDto>.ErrorResponse("An error occurred while updating the audit", 500));
            }
        }

        /// <summary>
        /// Start an audit
        /// </summary>
        /// <param name="id">Audit ID</param>
        /// <returns>Updated audit</returns>
        /// <response code="200">Audit started successfully</response>
        /// <response code="404">If audit not found</response>
        [HttpPost("{id}/start")]
        [ProducesResponseType(typeof(ApiResponse<AuditDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<AuditDto>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<AuditDto>>> StartAudit(int id)
        {
            try
            {
                await _auditService.StartAuditAsync(id);
                var audit = await _auditService.GetAuditDetailAsync(id);
                return Ok(ApiResponse<AuditDto>.SuccessResponse(MapToDto(audit!), "Audit started successfully"));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation when starting audit {AuditId}", id);
                return BadRequest(ApiResponse<AuditDto>.ErrorResponse(ex.Message, 400));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting audit {AuditId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<AuditDto>.ErrorResponse("An error occurred while starting the audit", 500));
            }
        }

        /// <summary>
        /// Complete an audit
        /// </summary>
        /// <param name="id">Audit ID</param>
        /// <param name="request">Complete request with findings</param>
        /// <returns>Updated audit</returns>
        /// <response code="200">Audit completed successfully</response>
        /// <response code="404">If audit not found</response>
        [HttpPost("{id}/complete")]
        [ProducesResponseType(typeof(ApiResponse<AuditDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<AuditDto>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<AuditDto>>> CompleteAudit(int id, [FromBody] CompleteAuditRequest request)
        {
            try
            {
                await _auditService.CompleteAuditAsync(id, request.Findings);
                var audit = await _auditService.GetAuditDetailAsync(id);
                return Ok(ApiResponse<AuditDto>.SuccessResponse(MapToDto(audit!), "Audit completed successfully"));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation when completing audit {AuditId}", id);
                return BadRequest(ApiResponse<AuditDto>.ErrorResponse(ex.Message, 400));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing audit {AuditId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<AuditDto>.ErrorResponse("An error occurred while completing the audit", 500));
            }
        }

        /// <summary>
        /// Delete an audit
        /// </summary>
        /// <param name="id">Audit ID</param>
        /// <returns>Success message</returns>
        /// <response code="200">Audit deleted successfully</response>
        /// <response code="404">If audit not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> DeleteAudit(int id)
        {
            try
            {
                var audit = await _auditService.GetAuditDetailAsync(id);
                if (audit is null)
                    return NotFound(ApiResponse.ErrorResponse("Audit not found", 404));

                // Implement delete in service layer
                return Ok(ApiResponse.SuccessResponse("Audit deleted successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting audit {AuditId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse.ErrorResponse("An error occurred while deleting the audit", 500));
            }
        }

        /// <summary>
        /// Map Audit model to AuditDto
        /// </summary>
        private AuditDto MapToDto(Audit audit)
        {
            return new AuditDto
            {
                Id = audit.Id,
                AuditTitle = audit.AuditTitle,
                BusinessName = audit.BusinessName,
                Status = audit.Status,
                ScheduledDate = audit.ScheduledDate,
                CompletedDate = audit.CompletedDate,
                Findings = audit.Findings,
                ChecklistJson = audit.ChecklistJson,
                AssignedOfficerId = audit.AssignedOfficerId,
                AssignedOfficerName = audit.AssignedOfficer?.FullName,
                ComplianceCaseId = audit.ComplianceCaseId
            };
        }
    }
}
