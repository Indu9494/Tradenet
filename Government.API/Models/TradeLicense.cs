namespace Goverment.Models
{
    public class TradeLicense
    {
        public int LicenseID { get; set; }
        public int BusinessID { get; set; }
        public string Type { get; set; } = string.Empty; // Import/Export/Local
        public DateTime IssuedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Status { get; set; } = string.Empty;

        public Business? Business { get; set; }
    }
}
