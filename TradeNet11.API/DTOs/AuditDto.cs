namespace TradeNet11.API.DTOs
{
    /// <summary>
    /// Audit Data Transfer Object for API responses
    /// </summary>
    public class AuditDto
    {
        public int Id { get; set; }
        public string AuditTitle { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
        public string Status { get; set; } = "Scheduled";
        public DateTime ScheduledDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string? Findings { get; set; }
        public string? ChecklistJson { get; set; }
        public int? AssignedOfficerId { get; set; }
        public string? AssignedOfficerName { get; set; }
        public int? ComplianceCaseId { get; set; }
    }

    /// <summary>
    /// Request model for creating an audit
    /// </summary>
    public class CreateAuditRequest
    {
        public string AuditTitle { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public int? AssignedOfficerId { get; set; }
        public int? ComplianceCaseId { get; set; }
        public string? ChecklistJson { get; set; }
    }

    /// <summary>
    /// Request model for updating an audit
    /// </summary>
    public class UpdateAuditRequest
    {
        public string AuditTitle { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public int? AssignedOfficerId { get; set; }
        public string? ChecklistJson { get; set; }
    }

    /// <summary>
    /// Request model for completing an audit
    /// </summary>
    public class CompleteAuditRequest
    {
        public string Findings { get; set; } = string.Empty;
    }
}
