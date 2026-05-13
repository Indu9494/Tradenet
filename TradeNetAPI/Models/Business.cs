namespace TradeNetAPI.Models
{
    public class Business
    {
        public int BusinessID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // Trader/Exporter/Importer
        public string Address { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending"; // Pending/Active/Inactive
        public string? RegistrationNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string ComplianceStatus { get; set; } = "Compliant"; // Compliant/Non-Compliant
    }
}
