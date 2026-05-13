namespace TradeNetAPI.Models
{
    public class BusinessDocument
    {
        public int DocumentID { get; set; }
        public int BusinessID { get; set; }
        public string DocType { get; set; } = string.Empty; // License/IDProof/TaxCertificate
        public string FileURI { get; set; } = string.Empty;
        public DateTime UploadedDate { get; set; }
        public string VerificationStatus { get; set; } = "Pending"; // Pending/Verified/Rejected
        public string? RejectionReason { get; set; }
    }
}
