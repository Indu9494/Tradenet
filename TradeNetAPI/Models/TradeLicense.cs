namespace TradeNetAPI.Models
{
    public class TradeLicense
    {
        public int LicenseID { get; set; }
        public int BusinessID { get; set; }
        public string Type { get; set; } = string.Empty; // Import/Export/Local
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? IssuedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Status { get; set; } = "Available"; // Available/Pending/Approved/Rejected
        public string? ApplicationStatus { get; set; } // PendingDocumentVerification/PendingComplianceCheck/Approved/RejectedDocumentError/RejectedComplianceError
        public decimal Fee { get; set; }
        public string? RejectionReason { get; set; }
        public DateTime? ApplicationDate { get; set; }
    }
}
