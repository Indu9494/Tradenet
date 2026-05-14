using System.ComponentModel.DataAnnotations;

namespace TradeNetProject.Models
{
    /// <summary>
    /// Unified User model combining all APIs
    /// Nullable fields for backward compatibility with all integrations
    /// </summary>
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "Business"; // Business/Officer/Manager/Admin/Compliance/Auditor

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;

        public string Status { get; set; } = "Active"; // Active/Inactive/Suspended

        public string? ProfilePicture { get; set; }

        // Authentication fields (from Government.API)
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }

        // Audit fields
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public DateTime? LastLoginDate { get; set; }
        public string? IpAddress { get; set; }

        // Business reference (nullable for officer/admin users)
        public string? BusinessName { get; set; }
    }
}
