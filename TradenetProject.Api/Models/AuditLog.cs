using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradeNetProject.Models
{
    /// <summary>
    /// Unified AuditLog model combining all APIs
    /// </summary>
    public class AuditLog
    {
        [Key]
        public int AuditID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        [Required]
        public string Action { get; set; } = string.Empty;

        [Required]
        public string Resource { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; } = DateTime.Now;

        public string? OldValues { get; set; }

        public string? NewValues { get; set; }

        public string? IpAddress { get; set; }

        // Navigation property
        public virtual User? User { get; set; }
    }
}
