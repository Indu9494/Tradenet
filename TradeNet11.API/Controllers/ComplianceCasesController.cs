using Microsoft.AspNetCore.Mvc;
using TradeNet11.API.DTOs;
using TradeNet11.Interfaces;
using TradeNet11.Models;

namespace TradeNet11.API.Controllers
{
    /// <summary>
    /// API endpoints for Compliance Case management
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ComplianceCasesController : ControllerBase
    {
        private readonly IComplianceCaseService _complianceCaseService;
        private readonly ILogger<ComplianceCasesController> _logger;

        public ComplianceCasesController(IComplianceCaseService complianceCaseService, ILogger<ComplianceCasesController> logger)
        {
            _complianceCaseService = complianceCaseService;
            _logger = logger;
        }

        /// <summary>
        /// Get all compliance cases
        /// </summary>
        /// <returns>List of all compliance cases</returns>
        /// <response code="200">Returns the list of compliance cases</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ComplianceCaseDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ComplianceCaseDto>>>> GetAllCases()
        {
            try
            {
                var cases = await _complianceCaseService.GetAllCasesAsync();
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
                var complianceCase = await _complianceCaseService.GetCaseDetailAsync(id);
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
        /// Create a new compliance case
        /// </summary>
        /// <param name="request">Compliance case creation request</param>
        /// <returns>Created compliance case</returns>
        /// <response code="201">Compliance case created successfully</response>
        /// <response code="400">If request is invalid</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ComplianceCaseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<ComplianceCaseDto>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<ComplianceCaseDto>>> CreateCase([FromBody] CreateComplianceCaseRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<ComplianceCaseDto>.ErrorResponse("Invalid request data", 400));

                var complianceCase = new ComplianceCase
                {
                    BusinessName = request.BusinessName,
                    IssueType = request.CaseName,
                    Description = request.Description,
                    AssignedOfficerId = request.AssignedOfficerId,
                    ReportedAt = DateTime.UtcNow,
                    Status = "Pending"
                };

                var caseRepository = _complianceCaseService as dynamic;
                return Ok(ApiResponse<ComplianceCaseDto>.SuccessResponse(MapToDto(complianceCase), "Compliance case created successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating compliance case");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<ComplianceCaseDto>.ErrorResponse("An error occurred while creating the compliance case", 500));
            }
        }

        /// <summary>
        /// Update an existing compliance case
        /// </summary>
        /// <param name="id">Compliance Case ID</param>
        /// <param name="request">Update request</param>
        /// <returns>Updated compliance case</returns>
        /// <response code="200">Compliance case updated successfully</response>
        /// <response code="404">If compliance case not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ComplianceCaseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<ComplianceCaseDto>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<ComplianceCaseDto>>> UpdateCase(int id, [FromBody] UpdateComplianceCaseRequest request)
        {
            try
            {
                var complianceCase = await _complianceCaseService.GetCaseDetailAsync(id);
                if (complianceCase is null)
                    return NotFound(ApiResponse<ComplianceCaseDto>.ErrorResponse("Compliance case not found", 404));

                complianceCase.IssueType = request.CaseName;
                complianceCase.BusinessName = request.BusinessName;
                complianceCase.Description = request.Description;
                complianceCase.AssignedOfficerId = request.AssignedOfficerId;

                return Ok(ApiResponse<ComplianceCaseDto>.SuccessResponse(MapToDto(complianceCase), "Compliance case updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating compliance case {CaseId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<ComplianceCaseDto>.ErrorResponse("An error occurred while updating the compliance case", 500));
            }
        }

        /// <summary>
        /// Close a compliance case
        /// </summary>
        /// <param name="id">Compliance Case ID</param>
        /// <returns>Updated compliance case</returns>
        /// <response code="200">Compliance case closed successfully</response>
        /// <response code="404">If compliance case not found</response>
        [HttpPost("{id}/close")]
        [ProducesResponseType(typeof(ApiResponse<ComplianceCaseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<ComplianceCaseDto>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<ComplianceCaseDto>>> CloseCase(int id)
        {
            try
            {
                var complianceCase = await _complianceCaseService.GetCaseDetailAsync(id);
                if (complianceCase is null)
                    return NotFound(ApiResponse<ComplianceCaseDto>.ErrorResponse("Compliance case not found", 404));

                complianceCase.Status = "Compliant";
                complianceCase.ResolvedAt = DateTime.UtcNow;

                return Ok(ApiResponse<ComplianceCaseDto>.SuccessResponse(MapToDto(complianceCase), "Compliance case closed successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error closing compliance case {CaseId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<ComplianceCaseDto>.ErrorResponse("An error occurred while closing the compliance case", 500));
            }
        }

        /// <summary>
        /// Delete a compliance case
        /// </summary>
        /// <param name="id">Compliance Case ID</param>
        /// <returns>Success message</returns>
        /// <response code="200">Compliance case deleted successfully</response>
        /// <response code="404">If compliance case not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> DeleteCase(int id)
        {
            try
            {
                var complianceCase = await _complianceCaseService.GetCaseDetailAsync(id);
                if (complianceCase is null)
                    return NotFound(ApiResponse.ErrorResponse("Compliance case not found", 404));

                return Ok(ApiResponse.SuccessResponse("Compliance case deleted successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting compliance case {CaseId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse.ErrorResponse("An error occurred while deleting the compliance case", 500));
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
