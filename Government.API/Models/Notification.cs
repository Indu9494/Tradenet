using Government.API.Models;

namespace Government.API.Models
{
    public class Notification
    {
        public int NotificationID { get; set; }
        public int UserID { get; set; }
        public int EntityID { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // License/Transaction/Program/Compliance
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }

        public User? User { get; set; }
    }
}

