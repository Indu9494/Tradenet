namespace TradeNetAPI.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = "Business"; // Business/Officer/Manager/Admin/Compliance/Auditor
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Status { get; set; } = "Active"; // Active/Inactive/Suspended
        public string? ProfilePicture { get; set; }
    }
}
