namespace TradeNet11.Models
{
    public class ComplianceRecord
    {
        public int Id { get; set; }
        public string ActionTaken { get; set; } = string.Empty;      // StatusChange, DocumentReview, AuditStarted, Remark, FlagRaised
        public string Details { get; set; } = string.Empty;
        public string? OfficerRemarks { get; set; }
        public string? RuleViolation { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public int ComplianceCaseId { get; set; }
        public ComplianceCase? ComplianceCase { get; set; }

        public int? OfficerId { get; set; }
        public ComplianceOfficer? Officer { get; set; }
    }
}
