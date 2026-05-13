namespace TradeNetAPI.Models
{
    public class TradeProgram
    {
        public int ProgramID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Budget { get; set; }
        public string Status { get; set; } = "Active"; // Active/Closed/Upcoming
        public string ProgramType { get; set; } = string.Empty; // Subsidy/Grant/ExportPromotion
        public string EligibilityCriteria { get; set; } = string.Empty;
    }
}
