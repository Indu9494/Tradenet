namespace TradeNetAPI.Models
{
    public class LicenseDocument
    {
        public int DocumentID { get; set; }
        public int LicenseID { get; set; }
        public string DocType { get; set; } = string.Empty; // Application/Approval/Certificate
        public string FileURI { get; set; } = string.Empty;
        public DateTime UploadedDate { get; set; }
        public string VerificationStatus { get; set; } = "Pending"; // Pending/Verified/Rejected
    }
}
