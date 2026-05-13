using System.ComponentModel.DataAnnotations;

namespace TradeNetProject.Models
{
    public class TradeLicense
    {
        [Key]
        public int LicenseID { get; set; }
        public string BusinessName { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }
}