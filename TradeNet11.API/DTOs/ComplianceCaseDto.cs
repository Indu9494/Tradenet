namespace TradeNet11.API.DTOs
{
    /// <summary>
    /// Compliance Case Data Transfer Object for API responses
    /// </summary>
    public class ComplianceCaseDto
    {
        public int Id { get; set; }
        public string CaseName { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public string? Description { get; set; }
        public int? AssignedOfficerId { get; set; }
        public string? AssignedOfficerName { get; set; }
    }

    /// <summary>
    /// Request model for creating a compliance case
    /// </summary>
    public class CreateComplianceCaseRequest
    {
        public string CaseName { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? AssignedOfficerId { get; set; }
    }

    /// <summary>
    /// Request model for updating a compliance case
    /// </summary>
    public class UpdateComplianceCaseRequest
    {
        public string CaseName { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? AssignedOfficerId { get; set; }
    }
}
