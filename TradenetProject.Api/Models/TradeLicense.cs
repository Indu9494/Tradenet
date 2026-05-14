using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradeNetProject.Models
{
    /// <summary>
    /// Unified TradeLicense model combining all APIs
    /// </summary>
    public class TradeLicense
    {
        [Key]
        public int LicenseID { get; set; }

        [ForeignKey("Business")]
        public int BusinessID { get; set; }

        [Required]
        public string Type { get; set; } = string.Empty; // Import/Export/Local

        [Required]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime? IssuedDate { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string Status { get; set; } = "Available"; // Available/Pending/Approved/Rejected

        public string? ApplicationStatus { get; set; } // PendingDocumentVerification/PendingComplianceCheck/Approved/RejectedDocumentError/RejectedComplianceError

        public decimal Fee { get; set; }

        public string? RejectionReason { get; set; }

        public DateTime? ApplicationDate { get; set; }

        // Legacy compatibility - old MVC used BusinessName instead of BusinessID
        public string? BusinessName { get; set; }

        // Navigation property
        public virtual Business? Business { get; set; }
    }
}
