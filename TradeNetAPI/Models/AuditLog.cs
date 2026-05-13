namespace TradeNetAPI.Models
{
    public class AuditLog
    {
        public int AuditID { get; set; }
        public int UserID { get; set; }
        public string Action { get; set; } = string.Empty;
        public string Resource { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string? IPAddress { get; set; }
    }
}
