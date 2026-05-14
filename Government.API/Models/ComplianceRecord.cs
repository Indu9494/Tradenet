namespace Government.API.Models
{
    public class ComplianceRecord
    {
        public int ComplianceID { get; set; }
        public int EntityID { get; set; }
        public string Type { get; set; } = string.Empty; // License/Transaction/Program
        public string Result { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}

