namespace Government.API.Models
{
    public class Report
    {
        public int ReportID { get; set; }
        public string Scope { get; set; } = string.Empty; // License/Transaction/Program
        public string Metrics { get; set; } = string.Empty;
        public DateTime GeneratedDate { get; set; }
    }
}

