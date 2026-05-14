using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradeNetProject.Models
{
    /// <summary>
    /// Unified Notification model combining all APIs
    /// </summary>
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        public int EntityID { get; set; }

        [Required]
        public string Message { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty; // License/Transaction/Program/Compliance

        public string Status { get; set; } = "Unread"; // Read/Unread

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? Priority { get; set; } = "Normal"; // Low/Normal/High

        // Navigation property
        public virtual User? User { get; set; }
    }
}
