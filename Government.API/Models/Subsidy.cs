namespace Government.API.Models
{
    public class Subsidy
    {
        public int SubsidyID { get; set; }
        public int BusinessID { get; set; }
        public int ProgramID { get; set; }
        public string Type { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? DisbursementDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? RejectionReason { get; set; }
        public string? Notes { get; set; }

        public virtual Business? Business { get; set; }
        public virtual TradeProgram? TradeProgram { get; set; }
    }
}

