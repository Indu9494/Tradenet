namespace TradeNet11.Models
{
    public class ComplianceCase
    {
        public int Id { get; set; }
        public string BusinessName { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public string IssueType { get; set; } = string.Empty;       // Violation, DocumentFraud, SuspiciousActivity, Expired, Mismatch
        public string Description { get; set; } = string.Empty;
        public string Severity { get; set; } = "Medium";            // Low, Medium, High, Critical
        public string Status { get; set; } = "Pending";             // Pending, Investigating, Compliant, NonCompliant, DocumentsRequested, AuditStarted
        public DateTime ReportedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ResolvedAt { get; set; }

        public int? AssignedOfficerId { get; set; }
        public ComplianceOfficer? AssignedOfficer { get; set; }

        public ICollection<ComplianceRecord> Records { get; set; } = new List<ComplianceRecord>();
    }
}
