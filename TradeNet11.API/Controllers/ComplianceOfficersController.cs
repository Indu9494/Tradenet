using Microsoft.AspNetCore.Mvc;
using TradeNet11.API.DTOs;
using TradeNet11.Interfaces;
using TradeNet11.Models;

namespace TradeNet11.API.Controllers
{
    /// <summary>
    /// API endpoints for Compliance Officer management
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ComplianceOfficersController : ControllerBase
    {
        private readonly IComplianceCaseRepository _caseRepository;
        private readonly ILogger<ComplianceOfficersController> _logger;

        public ComplianceOfficersController(IComplianceCaseRepository caseRepository, ILogger<ComplianceOfficersController> logger)
        {
            _caseRepository = caseRepository;
            _logger = logger;
        }

        /// <summary>
        /// Get all compliance cases (officers view)
        /// </summary>
        /// <returns>List of all compliance cases</returns>
        /// <response code="200">Returns the list of compliance cases</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ComplianceCaseDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ComplianceCaseDto>>>> GetAllCases()
        {
            try
            {
                var cases = await _caseRepository.GetAllAsync();
                var caseDtos = cases.Select(c => MapToDto(c)).ToList();
                return Ok(ApiResponse<IEnumerable<ComplianceCaseDto>>.SuccessResponse(caseDtos, "Compliance cases retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving compliance cases");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<IEnumerable<ComplianceCaseDto>>.ErrorResponse("An error occurred while retrieving compliance cases", 500));
            }
        }

        /// <summary>
        /// Get compliance case by ID
        /// </summary>
        /// <param name="id">Compliance Case ID</param>
        /// <returns>Compliance case details</returns>
        /// <response code="200">Returns the compliance case</response>
        /// <response code="404">If compliance case not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ComplianceCaseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<ComplianceCaseDto>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<ComplianceCaseDto>>> GetCaseById(int id)
        {
            try
            {
                var complianceCase = await _caseRepository.GetByIdAsync(id);
                if (complianceCase is null)
                    return NotFound(ApiResponse<ComplianceCaseDto>.ErrorResponse("Compliance case not found", 404));

                return Ok(ApiResponse<ComplianceCaseDto>.SuccessResponse(MapToDto(complianceCase), "Compliance case retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving compliance case {CaseId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<ComplianceCaseDto>.ErrorResponse("An error occurred while retrieving the compliance case", 500));
            }
        }

        /// <summary>
        /// Get cases by status
        /// </summary>
        /// <param name="status">Case status filter</param>
        /// <returns>Filtered compliance cases</returns>
        /// <response code="200">Returns the filtered compliance cases</response>
        [HttpGet("status/{status}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ComplianceCaseDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ComplianceCaseDto>>>> GetCasesByStatus(string status)
        {
            try
            {
                var cases = await _caseRepository.GetAllAsync();
                var filtered = cases.Where(c => c.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
                var caseDtos = filtered.Select(c => MapToDto(c)).ToList();
                return Ok(ApiResponse<IEnumerable<ComplianceCaseDto>>.SuccessResponse(caseDtos, $"Compliance cases with status '{status}' retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving compliance cases with status {Status}", status);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<IEnumerable<ComplianceCaseDto>>.ErrorResponse("An error occurred while retrieving compliance cases", 500));
            }
        }

        /// <summary>
        /// Update case status
        /// </summary>
        /// <param name="id">Compliance Case ID</param>
        /// <param name="newStatus">New status for the case</param>
        /// <returns>Updated compliance case</returns>
        /// <response code="200">Status updated successfully</response>
        /// <response code="404">If compliance case not found</response>
        [HttpPost("{id}/status")]
        [ProducesResponseType(typeof(ApiResponse<ComplianceCaseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<ComplianceCaseDto>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<ComplianceCaseDto>>> UpdateCaseStatus(int id, [FromQuery] string newStatus)
        {
            try
            {
                var complianceCase = await _caseRepository.GetByIdAsync(id);
                if (complianceCase is null)
                    return NotFound(ApiResponse<ComplianceCaseDto>.ErrorResponse("Compliance case not found", 404));

                complianceCase.Status = newStatus;
                await _caseRepository.UpdateAsync(complianceCase);

                return Ok(ApiResponse<ComplianceCaseDto>.SuccessResponse(MapToDto(complianceCase), "Case status updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating case status {CaseId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<ComplianceCaseDto>.ErrorResponse("An error occurred while updating the case status", 500));
            }
        }

        /// <summary>
        /// Map ComplianceCase model to ComplianceCaseDto
        /// </summary>
        private ComplianceCaseDto MapToDto(ComplianceCase complianceCase)
        {
            return new ComplianceCaseDto
            {
                Id = complianceCase.Id,
                CaseName = complianceCase.IssueType,
                BusinessName = complianceCase.BusinessName,
                Status = complianceCase.Status,
                CreatedDate = complianceCase.ReportedAt,
                ResolvedDate = complianceCase.ResolvedAt,
                Description = complianceCase.Description,
                AssignedOfficerId = complianceCase.AssignedOfficerId,
                AssignedOfficerName = complianceCase.AssignedOfficer?.FullName
            };
        }
    }
}
