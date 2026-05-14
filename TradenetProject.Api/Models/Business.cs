using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradeNetProject.Models
{
    /// <summary>
    /// Unified Business model combining all APIs
    /// </summary>
    public class Business
    {
        [Key]
        public int BusinessID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Type { get; set; } = string.Empty; // Trader/Exporter/Importer

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string ContactInfo { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending"; // Pending/Active/Inactive

        public string? RegistrationNumber { get; set; }
        
        public DateTime? RegistrationDate { get; set; }

        public string ComplianceStatus { get; set; } = "Compliant"; // Compliant/Non-Compliant

        // Navigation property
        public virtual User? User { get; set; }
    }
}
