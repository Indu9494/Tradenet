namespace TradeNet11.Models
{
    public class ProgramCompliance
    {
        public int Id { get; set; }
        public string ProgramName { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
        public string EligibilityStatus { get; set; } = "Pending";   // Pending, Eligible, NotEligible
        public string FundUsageSummary { get; set; } = string.Empty;
        public bool IsMisuseFlag { get; set; }
        public string? Remarks { get; set; }
        public DateTime ReviewedAt { get; set; } = DateTime.UtcNow;

        public int? ReviewedByOfficerId { get; set; }
        public ComplianceOfficer? ReviewedByOfficer { get; set; }
    }
}
