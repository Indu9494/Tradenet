namespace Goverment.Models
{
    public class Audit
    {
        public int AuditID { get; set; }
        public int OfficerID { get; set; }
        public string Scope { get; set; } = string.Empty;
        public string Findings { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;

        public User? Officer { get; set; }
    }
}
