namespace TradeNetAPI.Models
{
    public class ComplianceRecord
    {
        public int ComplianceID { get; set; }
        public int EntityID { get; set; }
        public string Type { get; set; } = string.Empty; // License/Transaction/Program
        public string Result { get; set; } = "Compliant"; // Compliant/Non-Compliant
        public DateTime Date { get; set; }
        public string Notes { get; set; } = string.Empty;
        public string? RecommendedAction { get; set; }
    }
}
