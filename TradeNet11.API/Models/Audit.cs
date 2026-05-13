namespace TradeNet11.Models
{
    public class Audit
    {
        public int Id { get; set; }
        public string AuditTitle { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
        public string Status { get; set; } = "Scheduled";           // Scheduled, InProgress, Completed, Cancelled
        public DateTime ScheduledDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string? Findings { get; set; }
        public string? ChecklistJson { get; set; }                   // JSON checklist items

        public int? AssignedOfficerId { get; set; }
        public ComplianceOfficer? AssignedOfficer { get; set; }

        public int? ComplianceCaseId { get; set; }
        public ComplianceCase? ComplianceCase { get; set; }
    }
}
