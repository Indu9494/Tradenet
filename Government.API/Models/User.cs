namespace Government.API.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // Business/Officer/Manager/Admin/Compliance/Auditor
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        // Authentication fields
        public string PasswordHash { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? LastLoginDate { get; set; }
        public string? BusinessName { get; set; }
    }
}

