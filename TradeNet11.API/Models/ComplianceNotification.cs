namespace TradeNet11.Models
{
    public class ComplianceNotification
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AlertType { get; set; } = string.Empty;   // Violation, DocumentIssue, SuspiciousTransaction, AuditDue, LicenseFlag
        public string Severity { get; set; } = "Medium";        // Low, Medium, High, Critical
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? ComplianceCaseId { get; set; }
        public ComplianceCase? ComplianceCase { get; set; }

        public int? AuditId { get; set; }
        public Audit? Audit { get; set; }

        public int? AssignedOfficerId { get; set; }
        public ComplianceOfficer? AssignedOfficer { get; set; }
    }
}
