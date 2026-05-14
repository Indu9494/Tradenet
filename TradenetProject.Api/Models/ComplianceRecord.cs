using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradeNetProject.Models
{
    /// <summary>
    /// Unified ComplianceRecord model combining all APIs
    /// </summary>
    public class ComplianceRecord
    {
        [Key]
        public int ComplianceID { get; set; }

        [ForeignKey("Business")]
        public int? EntityID { get; set; }

        public string? EntityType { get; set; } = "Business"; // Business/License/Transaction/Program

        public string Type { get; set; } = string.Empty;

        public string Result { get; set; } = string.Empty;

        public DateTime? Date { get; set; } = DateTime.Now;

        public string? Notes { get; set; }

        // Legacy fields - old MVC used these
        public string? BusinessName { get; set; }
        public string? InspectionType { get; set; }
        public string? InspectedDate { get; set; }
        public string? InspectedBy { get; set; }
        public string? Remarks { get; set; }

        // Navigation property
        public virtual Business? Business { get; set; }
    }
}
