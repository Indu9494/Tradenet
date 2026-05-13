using System.ComponentModel.DataAnnotations;

namespace TradeNetProject.Models
{
    public class MarketRecord
    {
        [Key]
        public int RecordID { get; set; }
        public string MarketName { get; set; }
        public string Location { get; set; }
        public int TotalVendors { get; set; }
        public string Category { get; set; }
        public double Revenue { get; set; }
        public string Status { get; set; }
    }
}
